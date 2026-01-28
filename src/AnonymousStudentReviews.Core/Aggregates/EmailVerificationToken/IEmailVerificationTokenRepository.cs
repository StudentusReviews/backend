using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;

public interface IEmailVerificationTokenRepository
{
    void Create(EmailVerificationToken emailVerificationToken);
    Task<Result<EmailVerificationToken>> GetByTokenHash(string tokenHash);
    void RedeemToken(EmailVerificationToken token);
}
