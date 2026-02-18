using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.UseCases.Universities.Delete;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Universities.Delete;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.Admin)]
[ApiController]
[Route("api/universities/{universityId:guid}")]
public class DeleteUniversityController : ControllerBase
{
    private readonly IDeleteUniversityService _service;

    public DeleteUniversityController(IDeleteUniversityService service)
    {
        _service = service;
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid universityId)
    {
        var result = await _service.ExecuteAsync(universityId);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return NoContent();
    }
}
