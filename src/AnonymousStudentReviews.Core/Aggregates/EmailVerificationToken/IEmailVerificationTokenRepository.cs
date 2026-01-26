namespace AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;

public interface IEmailVerificationTokenRepository
{
    void Create(EmailVerificationToken emailVerificationToken);
}
