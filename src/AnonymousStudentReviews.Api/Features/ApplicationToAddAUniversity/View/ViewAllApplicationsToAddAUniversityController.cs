using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.View;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications")]
public class ViewAllApplicationsToAddAUniversityController : ControllerBase
{
    private readonly IViewAllApplicationsToAddAUniversityService _viewAllApplicationToAddAUniversityService;

    public ViewAllApplicationsToAddAUniversityController(IViewAllApplicationsToAddAUniversityService viewAllApplicationToAddAUniversityService)
    {
        _viewAllApplicationToAddAUniversityService = viewAllApplicationToAddAUniversityService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>>> AllApplicationsToAddAUniniversity()
    {
        var viewApplicationToAddAUniversityResult = await _viewAllApplicationToAddAUniversityService.ExecuteAsync();
        if (viewApplicationToAddAUniversityResult.IsFailure)
        {
            return viewApplicationToAddAUniversityResult.Error.ToProblemDetails(Request.Path);
        }
        return Ok(viewApplicationToAddAUniversityResult.Value);
    }
}
