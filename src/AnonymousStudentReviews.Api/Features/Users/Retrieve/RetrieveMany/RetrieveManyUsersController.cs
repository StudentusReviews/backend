using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveMany;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve.RetrieveMany;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = "Admin"
)]
[Route("api/users")]
[ApiController]
public class RetrieveManyUsersController : ControllerBase
{
    private readonly IRetrieveManyUsersService _retrieveManyUsersService;
    private readonly IValidator<RetrieveManyUsersQueryParameters> _validator;

    public RetrieveManyUsersController(IRetrieveManyUsersService retrieveManyUsersService,
        IValidator<RetrieveManyUsersQueryParameters> validator)
    {
        _retrieveManyUsersService = retrieveManyUsersService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<UserPreview>>> GetUsersAsync(
        [FromQuery] RetrieveManyUsersQueryParameters queryParameters)
    {
        var validationResult = await _validator.ValidateAsync(queryParameters);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var result = await _retrieveManyUsersService.HandleAsync(QueryParametersToDto(queryParameters));

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(result.Value);
    }

    private RetrieveUsersDto QueryParametersToDto(RetrieveManyUsersQueryParameters queryParameters)
    {
        return new RetrieveUsersDto
        {
            QueryString = queryParameters.QueryString,
            UserId = queryParameters.UserId,
            UniversityId = queryParameters.UniversityId,
            UniversityName = queryParameters.UniversityName,
            Email = queryParameters.Email,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize,
            SortBy = queryParameters.SortBy,
            SortOrder = queryParameters.SortOrder
        };
    }
}
