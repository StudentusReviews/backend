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
    public IActionResult Index([FromQuery(Name = "return-url")] string? returnUrl = null)
    {
        ViewData["Title"] = "Реєстрація";
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromForm] RegistrationRequest request,
        [FromQuery(Name = "return-url")] string? returnUrl = null)
    {
        ViewData["Title"] = "Реєстрація";

        var validationResult = await _createUserRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View("Index", request);
        }

        var result = await _registrationService.HandleAsync(RequestToDto(request, returnUrl));

        if (result.IsFailure)
        {
            ViewData["ErrorName"] = "Error";
            return View("Index");
        }

        return View("Sucess");
    }

    private RegistrationDto RequestToDto(RegistrationRequest request, string? returnUrl)
    {
        returnUrl ??= "~/";
        return new RegistrationDto { Email = request.Email, Password = request.Password, ReturnUrl = returnUrl };
    }
}
