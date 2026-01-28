using System.Net.Mail;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AnonymousStudentReviews.UseCases.Users.Create;

public class CreateUserService : ICreateUserService
{
    private readonly IAccountVerificationLinkFactory _accountVerificationLinkFactory;
    private readonly IAllowedEmailDomainRepository _allowedEmailDomainRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailHasher _emailHasher;
    private readonly IEmailSender _emailSender;
    private readonly IEmailVerificationTokenGenerator _emailVerificationTokenGenerator;
    private readonly IEmailVerificationTokenHasher _emailVerificationTokenHasher;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly ILogger<CreateUserService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUserService(IAllowedEmailDomainRepository allowedEmailDomainRepository, IEmailHasher emailHasher,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository, IUnitOfWork unitOfWork, ILogger<CreateUserService> logger,
        IRoleRepository roleRepository, IEmailVerificationTokenRepository emailVerificationTokenRepository,
        IEmailVerificationTokenGenerator emailVerificationTokenGenerator,
        IEmailVerificationTokenHasher emailVerificationTokenHasher, IConfiguration configuration,
        IEmailSender emailSender, IAccountVerificationLinkFactory accountVerificationLinkFactory)
    {
        _allowedEmailDomainRepository = allowedEmailDomainRepository;
        _emailHasher = emailHasher;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _roleRepository = roleRepository;
        _emailVerificationTokenRepository = emailVerificationTokenRepository;
        _emailVerificationTokenGenerator = emailVerificationTokenGenerator;
        _emailVerificationTokenHasher = emailVerificationTokenHasher;
        _configuration = configuration;
        _emailSender = emailSender;
        _accountVerificationLinkFactory = accountVerificationLinkFactory;
    }

    public async Task<Result<User>> HandleAsync(CreateUserDto dto)
    {
        _logger.LogInformation("Create user service started");

        var emailHash = _emailHasher.Hash(dto.Email);

        if (await _userRepository.UserWithEmailHashExistsAsync(emailHash))
        {
            return Result.Failure<User>(CreateUserErrors.UserAlreadyExists);
        }

        var emailAddress = new MailAddress(dto.Email);
        var emailDomain = emailAddress.Host;

        var userIsConfirmedStudent = await _allowedEmailDomainRepository.IsEmailDomainAllowed(emailDomain);

        var hashedPassword = _passwordHasher.HashPassword(dto.Password);

        var createdUser = new User
        {
            Id = Guid.NewGuid(), EmailHash = emailHash, PasswordHash = hashedPassword, CreatedAt = DateTime.UtcNow
        };

        if (userIsConfirmedStudent)
        {
            _logger.LogInformation("User is a confirmed student");
            var universityId =
                (await _allowedEmailDomainRepository.FindByDomainAsync(emailDomain)).Value.UniversityId;
            createdUser.UniversityId = universityId;

            var getRoleResult = await _roleRepository.GetRoleByNameAsync(RoleNameConstants.Student);

            if (getRoleResult.IsFailure)
            {
                throw new InvalidOperationException("No role named \"Student\" found in the db");
            }

            var role = getRoleResult.Value;

            createdUser.Roles = [role];
        }

        var emailVerificationTokenString = _emailVerificationTokenGenerator.Generate();
        var emailVerificationTokenStringHash = _emailVerificationTokenHasher.Hash(emailVerificationTokenString);

        var emailVerificationTokenExpirationHoursString = _configuration["EmailVerificationTokenExpirationHours"];

        if (emailVerificationTokenExpirationHoursString is null)
        {
            throw new InvalidOperationException(
                "EmailVerificationTokenExpirationHours is null. EmailVerificationTokenExpirationHours must be set in appsettings.json or any other place where secrets reside");
        }

        var emailVerificationTokenExpirationHours = double.Parse(emailVerificationTokenExpirationHoursString);

        var emailVerificationToken = new EmailVerificationToken
        {
            Id = Guid.NewGuid(),
            TokenHash = emailVerificationTokenStringHash,
            ExpiresAt =
                DateTime.UtcNow.AddHours(emailVerificationTokenExpirationHours),
            CreatedAt = DateTime.UtcNow,
            User = createdUser
        };

        _emailVerificationTokenRepository.Create(emailVerificationToken);
        _userRepository.CreateUser(createdUser);

        await _unitOfWork.SaveChangesAsync();

        var accountVerificationLink = GenerateAccountVerificationLink(emailVerificationTokenString);
        await SendAccountVerificationEmailAsync(dto.Email, accountVerificationLink);

        return Result.Success(createdUser);
    }

    private string GenerateAccountVerificationLink(string emailVerificationToken)
    {
        return _accountVerificationLinkFactory.Create(emailVerificationToken);
    }

    private async Task SendAccountVerificationEmailAsync(string emailAddress, string accountVerificationLink)
    {
        await _emailSender.SendEmailAsync(
            "onboarding@resend.dev",
            [emailAddress],
            "Email verification",
            $"<a href=\"{accountVerificationLink}\">{accountVerificationLink}</a>"
        );
    }
}
