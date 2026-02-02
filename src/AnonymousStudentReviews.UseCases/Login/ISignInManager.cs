using AnonymousStudentReviews.Core.Aggregates.User;

namespace AnonymousStudentReviews.UseCases.Login;

public interface ISignInManager
{
    Task SignOutAsync();
    Task<bool> CanSignInAsync(User user);
}
