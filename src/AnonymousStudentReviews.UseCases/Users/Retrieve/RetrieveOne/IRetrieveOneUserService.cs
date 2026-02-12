using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveOne;

public interface IRetrieveOneUserService
{
    Task<Result<User>> HandleAsync(Guid userId);
}
