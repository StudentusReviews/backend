using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.Email;
using AnonymousStudentReviews.Infrastructure.Options;
using AnonymousStudentReviews.Infrastructure.Users;
using AnonymousStudentReviews.UseCases.Registration;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Microsoft.Extensions.Options;

using Moq;

namespace AnonymousStudentReviews.UnitTests.Users.Create;

// TODO: rewrite tests
public class UserManagerTests
{
    private readonly AccountConfirmationOptions _accountConfirmationOptions;

    private readonly Mock<IAccountVerificationLinkFactory> _accountVerificationLinkFactoryMock;
    private readonly Mock<IAllowedEmailDomainRepository> _allowedEmailDomainRepositoryMock;
    private readonly Mock<IEmailHasher> _emailHasherMock;
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly Mock<IEmailVerificationTokenGenerator> _emailVerificationTokenGeneratorMock;
    private readonly Mock<IEmailVerificationTokenHasher> _emailVerificationTokenHasherMock;
    private readonly Mock<IEmailVerificationTokenRepository> _emailVerificationTokenRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IRoleRepository> _roleRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UserManager _userManager;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IVerificationEmailGenerator> _verificationEmailGenerator;

    public UserManagerTests()
    {
        _accountConfirmationOptions = new AccountConfirmationOptions
        {
            EmailVerificationTokenExpirationHours = 24,
            SendConfirmationLetterFromAddress = "no-reply@test.com",
            ConfirmationLetterSubject = "Confirm your account"
        };

        _accountVerificationLinkFactoryMock = new Mock<IAccountVerificationLinkFactory>();
        _allowedEmailDomainRepositoryMock = new Mock<IAllowedEmailDomainRepository>();
        _emailHasherMock = new Mock<IEmailHasher>();
        _emailSenderMock = new Mock<IEmailSender>();
        _emailVerificationTokenGeneratorMock = new Mock<IEmailVerificationTokenGenerator>();
        _emailVerificationTokenHasherMock = new Mock<IEmailVerificationTokenHasher>();
        _emailVerificationTokenRepositoryMock = new Mock<IEmailVerificationTokenRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _roleRepositoryMock = new Mock<IRoleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _verificationEmailGenerator = new Mock<IVerificationEmailGenerator>();

        _userManager = new UserManager(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _passwordHasherMock.Object,
            _emailHasherMock.Object,
            _emailVerificationTokenGeneratorMock.Object,
            _emailVerificationTokenHasherMock.Object,
            Options.Create(_accountConfirmationOptions),
            _emailVerificationTokenRepositoryMock.Object,
            _accountVerificationLinkFactoryMock.Object,
            _emailSenderMock.Object,
            _allowedEmailDomainRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _verificationEmailGenerator.Object
        );
    }

    // [Fact]
    // public async Task CreateAsync_ShouldReturnFailure_WhenUserAlreadyExists()
    // {
    //     var email = "test@example.com";
    //     var password = "Password123!";
    //     var emailHash = "hashed_email";
    //
    //     _emailHasherMock.Setup(x => x.Hash(email)).Returns(emailHash);
    //
    //     _userRepositoryMock.Setup(x => x.UserWithEmailHashExistsAsync(emailHash))
    //         .ReturnsAsync(true);
    //
    //     var result = await _userManager.CreateAsync(email, password);
    //
    //     Assert.True(result.IsFailure);
    //     Assert.Equal(RegistrationErrors.UserAlreadyExists, result.Error);
    //
    //     _userRepositoryMock.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Never);
    //     _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Never);
    // }

    // [Fact]
    // public async Task CreateAsync_ShouldCreateUserAndAssignStudentRole_WhenDomainIsAllowed()
    // {
    //     var email = "student@university.edu";
    //     var password = "Password123!";
    //     var emailHash = "hashed_email";
    //     var passwordHash = "hashed_password";
    //     var universityId = Guid.NewGuid();
    //     var studentRole = new Role { Name = RoleNameConstants.Student };
    //
    //     _emailHasherMock.Setup(x => x.Hash(email)).Returns(emailHash);
    //     _userRepositoryMock.Setup(x => x.UserWithEmailHashExistsAsync(emailHash)).ReturnsAsync(false);
    //     _passwordHasherMock.Setup(x => x.HashPassword(password)).Returns(passwordHash);
    //
    //     _allowedEmailDomainRepositoryMock.Setup(x => x.IsEmailDomainAllowed("university.edu"))
    //         .ReturnsAsync(true);
    //
    //     var allowedDomainStub = new AllowedEmailDomain { UniversityId = universityId };
    //     _allowedEmailDomainRepositoryMock.Setup(x => x.FindByDomainAsync("university.edu"))
    //         .ReturnsAsync(Result.Success(allowedDomainStub));
    //
    //     _roleRepositoryMock.Setup(x => x.GetRoleByNameAsync(RoleNameConstants.Student))
    //         .ReturnsAsync(Result.Success(studentRole));
    //
    //     var result = await _userManager.CreateAsync(email, password);
    //
    //     Assert.True(result.IsSuccess);
    //     Assert.NotNull(result.Value);
    //     Assert.Equal(emailHash, result.Value.EmailHash);
    //     Assert.Equal(passwordHash, result.Value.PasswordHash);
    //     Assert.Equal(universityId, result.Value.UniversityId);
    //
    //     Assert.Contains(studentRole, result.Value.Roles);
    //
    //     _userRepositoryMock.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Once);
    //     _unitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    // }

    // [Fact]
    // public async Task CreateAsync_ShouldCreateUserWithoutRoles_WhenDomainIsNotAllowed()
    // {
    //     var email = "regular@gmail.com";
    //     var password = "Password123!";
    //
    //     _emailHasherMock.Setup(x => x.Hash(email)).Returns("hash");
    //     _userRepositoryMock.Setup(x => x.UserWithEmailHashExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
    //     _passwordHasherMock.Setup(x => x.HashPassword(password)).Returns("pass_hash");
    //
    //     _allowedEmailDomainRepositoryMock.Setup(x => x.IsEmailDomainAllowed("gmail.com"))
    //         .ReturnsAsync(false);
    //
    //     var result = await _userManager.CreateAsync(email, password);
    //
    //     Assert.True(result.IsSuccess);
    //     Assert.Empty(result.Value.Roles);
    //     Assert.Null(result.Value.UniversityId);
    // }

    // [Fact]
    // public async Task RequestAccountVerificationAsync_ShouldCreateTokenAndSendEmail()
    // {
    //     var user = new User { Id = Guid.NewGuid(), EmailHash = "hash" };
    //     var email = "test@example.com";
    //     var tokenString = "random-token-string";
    //     var tokenHash = "hashed-token";
    //     var link = "https://app.com/verify?t=...";
    //
    //     _emailVerificationTokenGeneratorMock.Setup(x => x.Generate()).Returns(tokenString);
    //     _emailVerificationTokenHasherMock.Setup(x => x.Hash(tokenString)).Returns(tokenHash);
    //
    //     _accountVerificationLinkFactoryMock.Setup(x => x.Create(tokenString)).Returns(link);
    //
    //     await _userManager.RequestAccountVerificationAsync(user, email);
    //
    //     _emailVerificationTokenRepositoryMock.Verify(x => x.Create(It.Is<EmailVerificationToken>(t =>
    //         t.TokenHash == tokenHash &&
    //         t.User == user &&
    //         t.ExpiresAt > DateTime.UtcNow
    //     )), Times.Once);
    //
    //     _unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    //
    //     _emailSenderMock.Verify(x => x.SendEmailAsync(
    //         _accountConfirmationOptions.SendConfirmationLetterFromAddress,
    //         It.Is<IEnumerable<string>>(emails => emails.Contains(email)),
    //         _accountConfirmationOptions.ConfirmationLetterSubject,
    //         It.Is<string>(body => body.Contains(link))
    //     ), Times.Once);
    // }
}
