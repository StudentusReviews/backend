using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve;

public interface IRetrieveUsersService
{
    Task<Result<PaginatedList<User>>> HandleAsync(RetrieveUsersDto dto);
}
