namespace AnonymousStudentReviews.UseCases.Registration.Abstractions;

public interface IEmailVerificationTokenGenerator
{
    string Generate();
}
