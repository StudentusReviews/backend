namespace AnonymousStudentReviews.UseCases.Users.Create;

public interface IEmailVerificationTokenHasher
{
    string Hash(string emailVerificationToken);
}
