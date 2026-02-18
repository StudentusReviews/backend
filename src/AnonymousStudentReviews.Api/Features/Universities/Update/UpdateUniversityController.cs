using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.UseCases.Universities.Update;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Universities.Update;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.Admin)]
[ApiController]
[Route("api/universities/{universityId:guid}")]
public class UpdateUniversityController : ControllerBase
{
    private readonly IValidator<UpdateUniversityRequest> _validator;
    private readonly IUpdateUniversityService _service;

    public UpdateUniversityController(IValidator<UpdateUniversityRequest> validator, IUpdateUniversityService service)
    {
        _validator = validator;
        _service = service;
    }

    [HttpPut]
    [ProducesResponseType(typeof(University), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<University>> Update(
        [FromRoute] Guid universityId,
        [FromBody] UpdateUniversityRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var dto = new UpdateUniversityDto
        {
            UniversityId = universityId,
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

        return Ok(result.Value);
    }
}
