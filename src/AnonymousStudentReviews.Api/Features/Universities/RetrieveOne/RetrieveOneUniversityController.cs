using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.UseCases.Universities.RetrieveOne;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Universities.RetrieveOne;

[Route("api/universities/{universityId:guid}")]
[ApiController]
[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.Admin)]
public class RetrieveOneUniversityController : ControllerBase
{
    private readonly IRetrieveOneUniversityService _retrieveOneUniversityService;

    public RetrieveOneUniversityController(IRetrieveOneUniversityService retrieveOneUniversityService)
    {
        _retrieveOneUniversityService = retrieveOneUniversityService;
    }

    [HttpGet]
    public async Task<ActionResult<UniversityDetailedPreview>> RetrieveOneUniversityAsync(
        [FromRoute] Guid universityId)
    {
        var result = await _retrieveOneUniversityService.HandleAsync(universityId);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(result.Value);
    }
}
