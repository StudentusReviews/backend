using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.AppToAddAUni.Create;
using AnonymousStudentReviews.UseCases.AppToAddAUni.View;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.AppToAddAUni.View;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications")]
public class ViewAppToAddAUniController : ControllerBase
{
    private readonly IViewAppToAddAUniService _viewAppToAddAUniService;

    public ViewAppToAddAUniController(IViewAppToAddAUniService viewAppToAddAUniService)
    {
        _viewAppToAddAUniService = viewAppToAddAUniService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Core.Aggregates.AppToAddAUni.AppToAddAUni>>> AllAppToAddAUni()
    {
        var viewAppToAddAUniResult = await _viewAppToAddAUniService.ExecuteAsync();
        if (viewAppToAddAUniResult.IsFailure)
        {
            return viewAppToAddAUniResult.Error.ToProblemDetails(Request.Path);
        }
        return Ok(viewAppToAddAUniResult.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Core.Aggregates.AppToAddAUni.AppToAddAUni>> AppToAddAUniById(Guid id)
    {
        var viewAppToAddAUniResult = await _viewAppToAddAUniService.ExecuteAsync(id);
        if (viewAppToAddAUniResult.IsFailure)
        {
            return viewAppToAddAUniResult.Error.ToProblemDetails(Request.Path);
        }
        return Ok(viewAppToAddAUniResult.Value);
    }
}
