namespace AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

public interface IAccountVerificationLinkFactory
{
    string Create(string emailVerificationToken);
}
