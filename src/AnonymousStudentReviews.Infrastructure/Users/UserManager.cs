using System.Net.Mail;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.UseCases.Users.Create;
using AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

using Microsoft.Extensions.Options;

namespace AnonymousStudentReviews.Infrastructure.Users;

public class UserManager : IUserManager
{
    private readonly AccountConfirmationOptions _accountConfirmationOptions;
    private readonly IAccountVerificationLinkFactory _accountVerificationLinkFactory;
    private readonly IAllowedEmailDomainRepository _allowedEmailDomainRepository;
    private readonly IEmailHasher _emailHasher;
    private readonly IEmailSender _emailSender;
    private readonly IEmailVerificationTokenGenerator _emailVerificationTokenGenerator;
    private readonly IEmailVerificationTokenHasher _emailVerificationTokenHasher;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public UserManager(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
        IEmailHasher emailHasher,
        IEmailVerificationTokenGenerator emailVerificationTokenGenerator,
        IEmailVerificationTokenHasher emailVerificationTokenHasher, IOptions<AccountConfirmationOptions> emailOptions,
        IEmailVerificationTokenRepository emailVerificationTokenRepository,
        IAccountVerificationLinkFactory accountVerificationLinkFactory, IEmailSender emailSender,
        IAllowedEmailDomainRepository allowedEmailDomainRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _emailHasher = emailHasher;
        _emailVerificationTokenGenerator = emailVerificationTokenGenerator;
        _emailVerificationTokenHasher = emailVerificationTokenHasher;
        _emailVerificationTokenRepository = emailVerificationTokenRepository;
        _accountVerificationLinkFactory = accountVerificationLinkFactory;
        _emailSender = emailSender;
        _allowedEmailDomainRepository = allowedEmailDomainRepository;
        _roleRepository = roleRepository;
        _accountConfirmationOptions = emailOptions.Value;
    }

    public async Task<Result<User>> CreateAsync(string email, string password)
    {
        var emailAddress = new MailAddress(email);
        var emailDomain = emailAddress.Host;

        var emailHash = _emailHasher.Hash(email);

        if (await _userRepository.UserWithEmailHashExistsAsync(emailHash))
        {
            return Result.Failure<User>(CreateUserErrors.UserAlreadyExists);
        }

        var hashedPassword = _passwordHasher.HashPassword(password);

        var createdUser = new User
        {
            Id = Guid.NewGuid(), EmailHash = emailHash, PasswordHash = hashedPassword, CreatedAt = DateTime.UtcNow
        };

        var userRoles = await DetermineUserRolesAsync(createdUser, email, emailDomain);

        createdUser.Roles = userRoles;

        _userRepository.CreateUser(createdUser);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success(createdUser);
    }

    public async Task RequestAccountVerificationAsync(User user, string email)
    {
        var emailVerificationTokenString = _emailVerificationTokenGenerator.Generate();
        var emailVerificationTokenStringHash = _emailVerificationTokenHasher.Hash(emailVerificationTokenString);

        var emailVerificationTokenExpirationHours = _accountConfirmationOptions.EmailVerificationTokenExpirationHours;

        var emailVerificationToken = new Core.Aggregates.EmailVerificationToken.EmailVerificationToken
        {
            Id = Guid.NewGuid(),
            TokenHash = emailVerificationTokenStringHash,
            ExpiresAt =
                DateTime.UtcNow.AddHours(emailVerificationTokenExpirationHours),
            CreatedAt = DateTime.UtcNow,
            User = user
        };

        _emailVerificationTokenRepository.Create(emailVerificationToken);
        await _unitOfWork.SaveChangesAsync();

        var accountVerificationLink = _accountVerificationLinkFactory.Create(emailVerificationTokenString);
        await SendAccountVerificationEmailAsync(email, accountVerificationLink);
    }

    private async Task SendAccountVerificationEmailAsync(string emailAddress, string accountVerificationLink)
    {
        await _emailSender.SendEmailAsync(
            _accountConfirmationOptions.SendConfirmationLetterFromAddress,
            [emailAddress],
            _accountConfirmationOptions.ConfirmationLetterSubject,
            $"<a href=\"{accountVerificationLink}\">{accountVerificationLink}</a>"
        );
    }

    private async Task<ICollection<Role>> DetermineUserRolesAsync(User user, string email, string emailDomain)
    {
        var userRoles = new List<Role>();

        var userIsConfirmedStudent = await _allowedEmailDomainRepository.IsEmailDomainAllowed(emailDomain);

        if (userIsConfirmedStudent)
        {
            var universityId =
                (await _allowedEmailDomainRepository.FindByDomainAsync(emailDomain)).Value.UniversityId;

            user.UniversityId = universityId;

            var getRoleResult = await _roleRepository.GetRoleByNameAsync(RoleNameConstants.Student);

            if (getRoleResult.IsFailure)
            {
                throw new InvalidOperationException("No role named \"Student\" found in the db");
            }

            userRoles.Add(getRoleResult.Value);
        }

        return userRoles;
    }
}
