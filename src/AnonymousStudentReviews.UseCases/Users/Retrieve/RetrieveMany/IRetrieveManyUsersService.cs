using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveMany;

public interface IRetrieveManyUsersService
{
    Task<Result<PaginatedList<UserPreview>>> HandleAsync(RetrieveUsersDto dto);
}
