using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Users.Create;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Registration;

[Route("api/register")]
public class RegistrationController : Controller
{
    private readonly IValidator<RegistrationRequest> _createUserRequestValidator;
    private readonly ICreateUserService _createUserService;

    public RegistrationController(IValidator<RegistrationRequest> createUserRequestValidator,
        ICreateUserService createUserService)
    {
        _createUserRequestValidator = createUserRequestValidator;
        _createUserService = createUserService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromForm] RegistrationRequest request)
    {
        var validationResult = await _createUserRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var result = await _createUserService.HandleAsync(RequestToDto(request));

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return View("Sucess");
    }

    private CreateUserDto RequestToDto(RegistrationRequest request)
    {
        return new CreateUserDto { Email = request.Email, Password = request.Password };
    }
}
