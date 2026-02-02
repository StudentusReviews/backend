using System.Security.Claims;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Registration.Abstractions;

public interface IUserManager
{
    Task<Result<User>> CreateAsync(string email, string password);
    Task RequestAccountVerificationAsync(User user, string email);
    Task<Result<User>> GetUserAsync(ClaimsPrincipal principal);
    Task<string> GetUserIdAsync(User user);
    Task<List<string>> GetRolesAsync(User user);
    Task<Result<User>> FindByIdAsync(string id);
    Task<Result<User>> FindByEmailAsync(string email);
}
