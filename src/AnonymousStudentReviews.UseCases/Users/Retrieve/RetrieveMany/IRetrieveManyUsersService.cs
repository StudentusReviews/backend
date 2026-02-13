using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveMany;

public interface IRetrieveManyUsersService
{
    Task<Result<PagedResponse<UserPreview>>> HandleAsync(RetrieveUsersDto dto);
}
