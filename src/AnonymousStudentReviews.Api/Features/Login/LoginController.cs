using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Login;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Login;

[Route("api/login")]
public class LoginController : Controller
{
    private readonly IValidator<LoginRequestQueryParameters> _loginRequestQueryParametersValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService, IValidator<LoginRequest> loginRequestValidator,
        IValidator<LoginRequestQueryParameters> loginRequestQueryParametersValidator)
    {
        _loginService = loginService;
        _loginRequestValidator = loginRequestValidator;
        _loginRequestQueryParametersValidator = loginRequestQueryParametersValidator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] LoginRequest request,
        [FromQuery] LoginRequestQueryParameters queryParameters)
    {
        var formValidationResult = await _loginRequestValidator.ValidateAsync(request);

        if (!formValidationResult.IsValid)
        {
            formValidationResult.AddToModelState(ModelState);
            return View("Index");
        }

        var queryValidationResult = await _loginRequestQueryParametersValidator.ValidateAsync(queryParameters);

        if (!queryValidationResult.IsValid || !Url.IsLocalUrl(queryParameters.ReturnUrl))
        {
            return BadRequest();
        }

        var loginResult = await _loginService.HandleAsync(RequestToDto(request));

        if (loginResult.IsFailure)
        {
            TempData["Error"] = "Invalid email or password";
            return View("Index");
        }

        return Redirect(queryParameters.ReturnUrl);
    }

    private LoginDto RequestToDto(LoginRequest request)
    {
        return new LoginDto { Email = request.Email, Password = request.Password, RememberMe = false };
    }
}
