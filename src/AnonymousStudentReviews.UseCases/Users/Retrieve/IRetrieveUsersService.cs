using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve;

public interface IRetrieveUsersService
{
    Task<Result<PaginatedList<UserPreview>>> HandleAsync(RetrieveUsersDto dto);
}
