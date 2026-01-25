using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.Users.Create;
using AnonymousStudentReviews.UseCases.Users.Create;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Register;

[Route("api/register")]
public class RegistrationController : Controller
{
    private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
    private readonly ICreateUserService _createUserService;

    public RegistrationController(IValidator<CreateUserRequest> createUserRequestValidator,
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
    public async Task<IActionResult> Register([FromForm] CreateUserRequest request)
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
    
    private CreateUserDto RequestToDto(CreateUserRequest request)
    {
        return new CreateUserDto { Email = request.Email, Password = request.Password };
    }
}
