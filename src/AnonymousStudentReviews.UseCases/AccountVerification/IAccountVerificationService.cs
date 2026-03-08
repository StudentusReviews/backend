using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.AccountVerification;

public interface IAccountVerificationService
{
    Task<Result> HandleAsync(string emailVerificationToken);
}
