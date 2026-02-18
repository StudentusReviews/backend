using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.UseCases.Universities.RetrieveMany;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Universities.RetrieveMany;

[Route("api/universities")]
[ApiController]
[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.Admin)]
public class RetrieveManyUniversitiesController : ControllerBase
{
    private readonly IRetrieveManyUniversitiesService _retrieveManyUniversitiesService;
    private readonly IValidator<RetrieveManyUniversitiesQueryParameters> _validator;


    public RetrieveManyUniversitiesController(IValidator<RetrieveManyUniversitiesQueryParameters> validator,
        IRetrieveManyUniversitiesService retrieveManyUniversitiesService)
    {
        _validator = validator;
        _retrieveManyUniversitiesService = retrieveManyUniversitiesService;
    }

    [HttpGet]
    public async Task<ActionResult> RetrieveAllUniversitiesAsync(
        [FromQuery] RetrieveManyUniversitiesQueryParameters queryParameters)

    {
        var validationResult = await _validator.ValidateAsync(queryParameters);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var result = await _retrieveManyUniversitiesService.HandleAsync(RequestToDto(queryParameters));

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(result.Value);
    }

    private RetrieveManyUniversitiesDto RequestToDto(RetrieveManyUniversitiesQueryParameters request)
    {
        return new RetrieveManyUniversitiesDto
        {
            Limit = request.Limit,
            Offset = request.Offset,
            Query = request.Query,
            Name = request.Name,
            City = request.City,
            UniversitySortBy = request.SortBy,
            SortOrder = request.SortOrder
        };
    }
}
