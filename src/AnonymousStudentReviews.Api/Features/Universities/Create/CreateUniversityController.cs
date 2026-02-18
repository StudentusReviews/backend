using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.UseCases.Universities.Create;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Universities.Create;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.Admin)]
[ApiController]
[Route("api/universities")]
public class CreateUniversityController : ControllerBase
{
    private readonly IValidator<CreateUniversityRequest> _validator;
    private readonly ICreateUniversityService _service;

    public CreateUniversityController(IValidator<CreateUniversityRequest> validator, ICreateUniversityService service)
    {
        _validator = validator;
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(University), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<University>> Create([FromBody] CreateUniversityRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var dto = new CreateUniversityDto
        {
            Name = request.Name,
            City = request.City,
            Website = request.Website,
            Description = request.Description
        };

        var result = await _service.ExecuteAsync(dto);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return CreatedAtAction(nameof(Create), result.Value);
    }
}
