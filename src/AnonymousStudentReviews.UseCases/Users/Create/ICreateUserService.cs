using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Create;

public interface ICreateUserService
{
    Task<Result<User>> HandleAsync(CreateUserDto dto);
}
