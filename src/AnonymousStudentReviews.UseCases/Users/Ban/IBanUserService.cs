using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Users.Ban;

public interface IBanUserService
{
    Task<Result> HandleAsync(Guid userId);
}
