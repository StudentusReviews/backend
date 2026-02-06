using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Login.Abstractions;

public interface ISignInManager
{
    Task SignOutAsync();
    Task<bool> CanSignInAsync(User user);
    Task<Result> CheckPasswordSignInAsync(User user, string password);
    Task SignInAsync(User user, bool isPersistent);
}
