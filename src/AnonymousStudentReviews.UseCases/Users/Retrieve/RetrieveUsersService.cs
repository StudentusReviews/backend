using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Retrieve;

public class RetrieveUsersService : IRetrieveUsersService
{
    public Task<Result<PaginatedList<User>>> HandleAsync(RetrieveUsersDto dto)
    {
        throw new NotImplementedException();
    }
}
