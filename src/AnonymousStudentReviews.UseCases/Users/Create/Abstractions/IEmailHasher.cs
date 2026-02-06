namespace AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

public interface IEmailHasher
{
    public string Hash(string email);
}
