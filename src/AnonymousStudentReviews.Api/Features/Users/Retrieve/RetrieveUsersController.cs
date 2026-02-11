using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.UseCases.Users.Retrieve;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve;

[Route("api/users")]
[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = "Admin"
)]
[ApiController]
public class RetrieveUsersController : ControllerBase
{
    private readonly IRetrieveUsersService _retrieveUsersService;

    public RetrieveUsersController(IRetrieveUsersService retrieveUsersService)
    {
        _retrieveUsersService = retrieveUsersService;
    }

    public async Task<ActionResult<PaginatedList<RetrieveUserResponse>>> GetUsersAsync(
        [FromQuery] RetrieveUsersQueryParameters queryParameters)
    {
        var result = await _retrieveUsersService.HandleAsync(QueryParametersToDto(queryParameters));

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok();
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
            PageSize = queryParameters.PageSize
        };
    }
}
