namespace AnonymousStudentReviews.UseCases.Registration.Abstractions;

public interface IAccountVerificationLinkFactory
{
    string Create(string emailVerificationToken);
}
