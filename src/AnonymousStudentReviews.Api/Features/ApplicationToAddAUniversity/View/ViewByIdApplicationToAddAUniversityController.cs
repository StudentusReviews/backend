using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.Create;
using AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.View;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.ApplicationToAddAUniversity.View;

[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/applications")]
public class ViewByIdApplicationToAddAUniversityController : Controller
{
    private readonly IViewByIdApplicationToAddAUniversityService _viewByIdApplicationToAddAUniversityService;

    public ViewByIdApplicationToAddAUniversityController(IViewByIdApplicationToAddAUniversityService viewByIdApplicationToAddAUniversityService)
    {
        _viewByIdApplicationToAddAUniversityService = viewByIdApplicationToAddAUniversityService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>> ApplicationToAddAUniversityById(Guid id)
    {
        var viewByIdApplicationToAddAUniversityResult = await _viewByIdApplicationToAddAUniversityService.ExecuteAsync(id);
        if (viewByIdApplicationToAddAUniversityResult.IsFailure)
        {
            return viewByIdApplicationToAddAUniversityResult.Error.ToProblemDetails(Request.Path);
        }
        return Ok(viewByIdApplicationToAddAUniversityResult.Value);
    }
}
