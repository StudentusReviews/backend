namespace AnonymousStudentReviews.Infrastructure.Email;

public interface IVerificationEmailGenerator
{
    Task<string> GenerateVerificationEmailAsync(string verificationLink);
}
