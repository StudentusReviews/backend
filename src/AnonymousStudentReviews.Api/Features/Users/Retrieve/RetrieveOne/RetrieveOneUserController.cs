using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Users.Retrieve.RetrieveOne;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Validation.AspNetCore;

namespace AnonymousStudentReviews.Api.Features.Users.Retrieve.RetrieveOne;

[Authorize(
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
    Roles = "Admin"
)]
[Route("api/users/{userId:guid}")]
[ApiController]
public class RetrieveOneUserController : ControllerBase
{
    private readonly IRetrieveOneUserService _retrieveOneUserService;

    public RetrieveOneUserController(IRetrieveOneUserService service)
    {
        _retrieveOneUserService = service;
    }

    [HttpGet]
    public async Task<ActionResult<RetrieveOneUserResponse>> RetrieveOneUserAsync([FromRoute] Guid userId)
    {
        var result = await _retrieveOneUserService.HandleAsync(userId);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return Ok(ResultToResponse(result.Value));
    }

    private RetrieveOneUserResponse ResultToResponse(User result)
    {
        var response = new RetrieveOneUserResponse
        {
            Id = result.Id,
            UniversityId = result.UniversityId,
            RegistrationDate = result.CreatedAt,
            EmailConfirmed = result.EmailConfirmed,
            AccessFailedCount = result.AccessFailedCount,
            LockoutEnd = result.LockoutEnd,
            Roles = result.Roles.Select(role => new RolePreview { Id = role.Id, Name = role.Name }),
            UniversityName = result.University?.Name,
            IsBanned = result.IsBanned
        };

        return response;
    }
}
