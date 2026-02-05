using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Users.Create.Abstractions;

public interface IUserManager
{
    Task<Result<User>> CreateAsync(string email, string password);
    Task RequestAccountVerificationAsync(User user, string email);
}
