namespace AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

public interface IEmailVerificationTokenHasher
{
    string Hash(string emailVerificationToken);
}
