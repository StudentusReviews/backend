namespace AnonymousStudentReviews.UseCases.Registration.Abstractions;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}
