using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Users.Retrieve;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve;

// [Authorize(
//     AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
//     Roles = "Admin"
// )]
[Route("api/users")]
[ApiController]
public class RetrieveUsersController : ControllerBase
{
    private readonly IRetrieveUsersService _retrieveUsersService;
    private readonly IValidator<RetrieveUsersQueryParameters> _validator;

    public RetrieveUsersController(IRetrieveUsersService retrieveUsersService,
        IValidator<RetrieveUsersQueryParameters> validator)
    {
        _retrieveUsersService = retrieveUsersService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserPreview>>> GetUsersAsync(
        [FromQuery] RetrieveUsersQueryParameters queryParameters)
    {
        var validationResult = await _validator.ValidateAsync(queryParameters);

        if (!validationResult.IsValid)
        {
            return validationResult.ToProblemDetails(Request.Path);
        }

        var result = await _retrieveUsersService.HandleAsync(QueryParametersToDto(queryParameters));

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(result.Value);
    }

    private RetrieveUsersDto QueryParametersToDto(RetrieveUsersQueryParameters queryParameters)
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
