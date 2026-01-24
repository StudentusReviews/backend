using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Users.Create;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Users.Create;

[ApiController]
[Route("api/users")]
public class CreateUserController : ControllerBase
{
    private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
    private readonly ICreateUserService _createUserService;

    public CreateUserController(IValidator<CreateUserRequest> createUserRequestValidator,
        ICreateUserService createUserService)
    {
        _createUserRequestValidator = createUserRequestValidator;
        _createUserService = createUserService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
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

        // return CreatedAtAction(nameof(CreateUserAsync),
        //     new { result.Value.Id }, result.Value);
        return Created();
    }

    private CreateUserDto RequestToDto(CreateUserRequest request)
    {
        return new CreateUserDto { Email = request.Email, Password = request.Password };
    }
}
