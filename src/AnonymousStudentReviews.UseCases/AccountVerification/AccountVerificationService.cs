using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Users.Create;

namespace AnonymousStudentReviews.UseCases.AccountVerification;

public class AccountVerificationService : IAccountVerificationService
{
    private readonly IEmailVerificationTokenHasher _emailVerificationTokenHasher;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public AccountVerificationService(IEmailVerificationTokenRepository emailVerificationTokenRepository,
        IUnitOfWork unitOfWork, IEmailVerificationTokenHasher emailVerificationTokenHasher,
        IUserRepository userRepository)
    {
        _emailVerificationTokenRepository = emailVerificationTokenRepository;
        _unitOfWork = unitOfWork;
        _emailVerificationTokenHasher = emailVerificationTokenHasher;
        _userRepository = userRepository;
    }

    public async Task<Result> HandleAsync(string emailVerificationToken)
    {
        var emailVerificationTokenHash = _emailVerificationTokenHasher.Hash(emailVerificationToken);

        var getEmailVerificationTokenResult =
            await _emailVerificationTokenRepository.GetByTokenHash(emailVerificationTokenHash);

        if (getEmailVerificationTokenResult.IsFailure)
        {
            return Result.Failure(AccountVerificationErrors.TokenNotFound);
        }

        var emailVerificationTokenEntity = getEmailVerificationTokenResult.Value;

        if (emailVerificationTokenEntity.ExpiresAt >= DateTime.UtcNow)
        {
            return Result.Failure(AccountVerificationErrors.TokenExpired);
        }

        if (emailVerificationTokenEntity.UsedAt is not null)
        {
            return Result.Failure(AccountVerificationErrors.TokenAlreadyUsed);
        }

        _emailVerificationTokenRepository.RedeemToken(emailVerificationTokenEntity);
        _userRepository.ConfirmUser(emailVerificationTokenEntity.User);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
