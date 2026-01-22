using AnonymousStudentReviews.Api.Extensions;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Users.Create;

[ApiController]
[Route("api/users")]
public class CreateUserController : ControllerBase
{
    private readonly IValidator<CreateUserRequest> _createUserRequestValidator;

    public CreateUserController(IValidator<CreateUserRequest> createUserRequestValidator)
    {
        _createUserRequestValidator = createUserRequestValidator;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var validationResult = await _createUserRequestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var id = 0;
        var newItem = new { };

        return CreatedAtAction(nameof(CreateUserAsync), new { id }, newItem);
    }
}
