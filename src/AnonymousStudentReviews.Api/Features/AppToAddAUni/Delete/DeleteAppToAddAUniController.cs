using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.AppToAddAUni.Delete;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

using AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;


namespace AnonymousStudentReviews.Api.Features.AppToAddAUni.Delete;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications")]
public class DeleteAppToAddAUniController : ControllerBase
{
    private readonly IDeleteAppToAddAUniService _deleteAppToAddAUniService;

    public DeleteAppToAddAUniController(IDeleteAppToAddAUniService deleteAppToAddAUniService)
        {
            _deleteAppToAddAUniService = deleteAppToAddAUniService;
        }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteAppToAddAUni([FromQuery] Guid appId)
    {
        var deleteAppToAddAUniResult = await _deleteAppToAddAUniService.ExecuteAsync(appId);
        if (deleteAppToAddAUniResult.IsFailure)
        {
            return deleteAppToAddAUniResult.Error.ToProblemDetails(Request.Path);
        }
        return NoContent();
    }

}
