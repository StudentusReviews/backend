namespace AnonymousStudentReviews.UseCases.Users.Create;

public interface IEmailHasher
{
    public string Hash(string email);
}
