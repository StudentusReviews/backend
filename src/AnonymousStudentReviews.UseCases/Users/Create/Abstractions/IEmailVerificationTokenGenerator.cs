namespace AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

public interface IEmailVerificationTokenGenerator
{
    string Generate();
}
