using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.Create;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.View;

//[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications")]
public class ViewApplicationToAddAUniversityController : ControllerBase
{
    private readonly IViewApplicationToAddAUniversityService _viewApplicationToAddAUniversityService;

    public ViewApplicationToAddAUniversityController(IViewApplicationToAddAUniversityService viewApplicationToAddAUniversityService)
    {
        _viewApplicationToAddAUniversityService = viewApplicationToAddAUniversityService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>>> AllAppToAddAUni()
    {
        var viewApplicationToAddAUniversityResult = await _viewApplicationToAddAUniversityService.ExecuteAsync();
        if (viewApplicationToAddAUniversityResult.IsFailure)
        {
            return viewApplicationToAddAUniversityResult.Error.ToProblemDetails(Request.Path);
        }
        return Ok(viewApplicationToAddAUniversityResult.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>> AppToAddAUniById(Guid id)
    {
        var viewApplicationToAddAUniversityResult = await _viewApplicationToAddAUniversityService.ExecuteAsync(id);
        if (viewApplicationToAddAUniversityResult.IsFailure)
        {
            return viewApplicationToAddAUniversityResult.Error.ToProblemDetails(Request.Path);
        }
        return Ok(viewApplicationToAddAUniversityResult.Value);
    }
}
