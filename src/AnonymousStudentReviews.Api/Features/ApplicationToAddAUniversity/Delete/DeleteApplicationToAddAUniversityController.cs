using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Delete;

using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;


using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity;
using Microsoft.AspNetCore.Authorization;

namespace AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.Delete;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications/delete")]
public class DeleteApplicationToAddAUniversityController : ControllerBase
{
    private readonly IDeleteApplicationToAddAUniversityService _deleteApplicationToAddAUniversityService;

    public DeleteApplicationToAddAUniversityController(IDeleteApplicationToAddAUniversityService deleteApplicationToAddAUniversityService)
    {
        _deleteApplicationToAddAUniversityService = deleteApplicationToAddAUniversityService;
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteApplicationToAddAUniversity([FromQuery] Guid appId)
    {
        var deleteApplicationToAddAUniversityResult = await _deleteApplicationToAddAUniversityService.ExecuteAsync(appId);
        if (deleteApplicationToAddAUniversityResult.IsFailure)
        {
            return deleteApplicationToAddAUniversityResult.Error.ToProblemDetails(Request.Path);
        }
        return NoContent();
    }

}
