using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Registration;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Registration;

[Route("api/register")]
public class RegistrationController : Controller
{
    private readonly IValidator<RegistrationRequest> _createUserRequestValidator;
    private readonly IRegistrationService _registrationService;

    public RegistrationController(IValidator<RegistrationRequest> createUserRequestValidator,
        IRegistrationService registrationService)
    {
        _createUserRequestValidator = createUserRequestValidator;
        _registrationService = registrationService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([FromForm] RegistrationRequest request)
    {
        var validationResult = await _createUserRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(request);
        }

        var result = await _registrationService.HandleAsync(RequestToDto(request));

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return View("Sucess");
    }

    private RegistrationDto RequestToDto(RegistrationRequest request)
    {
        return new RegistrationDto { Email = request.Email, Password = request.Password };
    }
}
