namespace AnonymousStudentReviews.UseCases.Registration.Abstractions;

public interface IEmailVerificationTokenHasher
{
    string Hash(string emailVerificationToken);
}
