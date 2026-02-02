using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Login;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Login;

[Route("api/login")]
public class LoginController : Controller
{
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService, IValidator<LoginRequest> loginRequestValidator)
    {
        _loginService = loginService;
        _loginRequestValidator = loginRequestValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        var validationResult = await _loginRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View("Index");
        }

        var loginResult = await _loginService.HandleAsync(RequestToDto(request));

        if (loginResult.IsFailure)
        {
            TempData["Error"] = "Invalid email or password";
            return View("Index");
        }

        return Redirect("/connect/authorize");
    }

    private LoginDto RequestToDto(LoginRequest request)
    {
        return new LoginDto { Email = request.Email, Password = request.Password, RememberMe = false };
    }
}
