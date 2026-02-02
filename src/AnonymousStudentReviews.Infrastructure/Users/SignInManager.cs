using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Login;

namespace AnonymousStudentReviews.Infrastructure.Users;

public class SignInManager : ISignInManager
{
    public async Task SignOutAsync()
    {
        // TODO: implement the SignOutAsync method properly
    }

    public async Task<bool> CanSignInAsync(User user)
    {
        // TODO: implement the CanSignAsync method properly
        return true;
    }
}
