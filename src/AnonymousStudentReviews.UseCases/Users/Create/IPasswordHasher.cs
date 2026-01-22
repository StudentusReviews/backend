namespace AnonymousStudentReviews.UseCases.Users.Create;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}
