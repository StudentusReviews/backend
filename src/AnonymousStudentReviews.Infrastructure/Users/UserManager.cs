using System.Net.Mail;
using System.Security.Claims;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.UseCases.Registration;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;

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
            return Result.Failure<User>(RegistrationErrors.UserAlreadyExists);
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

    public async Task<Result<User>> GetUserAsync(ClaimsPrincipal principal)
    {
        var subjectClaim = principal.Claims.FirstOrDefault(claim => claim.Type == OpenIddictConstants.Claims.Subject);

        if (subjectClaim is null)
        {
            throw new InvalidOperationException("The user details cannot be retrieved.");
        }

        var userIdString = subjectClaim.Value;
        
        var parseIdResult = Guid.TryParse(userIdString, out var userId);

        if (!parseIdResult)
        {
            throw new InvalidOperationException("The user id is in the wrong format");
        }

        var findUserResult = await _userRepository.FindByIdAsync(userId);

        if (findUserResult.IsFailure)
        {
            return Result.Failure<User>(findUserResult.Error);
        }

        return findUserResult.Value;
    }

    public async Task<string> GetUserIdAsync(User user)
    {
        return user.Id.ToString();
    }

    public async Task<List<string>> GetRolesAsync(User user)
    {
        var roles = await _userRepository.GetRolesAsync(user);
        var roleNames = roles.Select(role => role.Name).ToList();
        return roleNames;
    }

    public async Task<Result<User>> FindByIdAsync(string id)
    {
        var parseIdResult = Guid.TryParse(id, out var userId);

        if (!parseIdResult)
        {
            throw new InvalidOperationException("The user id is in the wrong format");
        }

        return await _userRepository.FindByIdAsync(userId);
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
