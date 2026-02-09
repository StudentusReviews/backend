using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Registration;

public interface IRegistrationService
{
    Task<Result> HandleAsync(RegistrationDto dto);
}
