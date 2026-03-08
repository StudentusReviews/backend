using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.UseCases.Login.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.Login;

public class LoginService : ILoginService
{
    private readonly ISignInManager _signInManager;
    private readonly IUserManager _userManager;

    public LoginService(IUserManager userManager, ISignInManager signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result> HandleAsync(LoginDto dto)
    {
        var findUserResult = await _userManager.FindByEmailAsync(dto.Email);

        if (findUserResult.IsFailure)
        {
            return Result.Failure(LoginErrors.WrongEmailOrPassword);
        }

        var user = findUserResult.Value;

        var canSignIn = await _signInManager.CanSignInAsync(user);

        if (!canSignIn)
        {
            return Result.Failure(LoginErrors.WrongEmailOrPassword);
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(findUserResult.Value, dto.Password);

        if (signInResult.IsFailure)
        {
            return Result.Failure(LoginErrors.WrongEmailOrPassword);
        }

        await _signInManager.SignInAsync(user, dto.RememberMe);
        return Result.Success();
    }
}
