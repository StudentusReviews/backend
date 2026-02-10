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
    public IActionResult Index(string? returnUrl = null)
    {
        ViewData["Title"] = "Вхід";
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginRequest request, string? returnUrl = null)
    {
        ViewData["Title"] = "Вхід";

        var formValidationResult = await _loginRequestValidator.ValidateAsync(request);

        if (!formValidationResult.IsValid)
        {
            formValidationResult.AddToModelState(ModelState);
            return View("Index");
        }

        returnUrl ??= "~/";

        var loginResult = await _loginService.HandleAsync(RequestToDto(request));

        if (loginResult.IsFailure)
        {
            ViewData["ErrorName"] = "Помилка входу";
            ViewData["ErrorDescription"] = "Невірний логін або пароль";
            return View("Index");
        }

        return LocalRedirect(returnUrl);
    }

    private LoginDto RequestToDto(LoginRequest request)
    {
        return new LoginDto { Email = request.Email, Password = request.Password, RememberMe = false };
    }
}
