using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.User;

using Microsoft.Extensions.Logging;

namespace AnonymousStudentReviews.UseCases.Users.Create;

public class CreateUserService : ICreateUserService
{
    private readonly IAllowedEmailDomainRepository _allowedEmailDomainRepository;
    private readonly IEmailHasher _emailHasher;
    private readonly ILogger<CreateUserService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public CreateUserService(IAllowedEmailDomainRepository allowedEmailDomainRepository, IEmailHasher emailHasher,
        IPasswordHasher passwordHasher,
        IUserRepository userRepository, IUnitOfWork unitOfWork, ILogger<CreateUserService> logger)
    {
        _allowedEmailDomainRepository = allowedEmailDomainRepository;
        _emailHasher = emailHasher;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<User>> HandleAsync(CreateUserDto dto)
    {
        _logger.LogInformation("Create user service started");
        
        var emailHash = _emailHasher.Hash(dto.Email);

        if (await _userRepository.UserWithEmailHashExistsAsync(emailHash))
        {
            return Result.Failure<User>(CreateUserErrors.UserAlreadyExists);
        }

        var emailDomain = dto.Email.Split("@")[1];

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
        }

        _userRepository.CreateUser(createdUser);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(createdUser);
    }
}
