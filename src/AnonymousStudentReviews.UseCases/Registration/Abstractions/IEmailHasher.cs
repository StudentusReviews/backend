namespace AnonymousStudentReviews.UseCases.Registration.Abstractions;

public interface IEmailHasher
{
    public string Hash(string email);
}
