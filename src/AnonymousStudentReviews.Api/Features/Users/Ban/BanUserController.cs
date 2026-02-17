using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Users.Ban;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace AnonymousStudentReviews.Api.Features.Users.Ban;

[Route("api/users/{userId:guid}/ban")]
[Authorize(
    AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
    Roles = "Admin,SuperAdmin"
)]
[ApiController]
public class BanUserController : ControllerBase
{
    private readonly IBanUserService _banUserService;

    public BanUserController(IBanUserService banUserService)
    {
        _banUserService = banUserService;
    }

    [HttpPost]
    public async Task<ActionResult> BanUserAsync([FromRoute] Guid userId)
    {
        var banUserResult = await _banUserService.HandleAsync(userId);

        if (banUserResult.IsFailure)
        {
            return banUserResult.Error.ToProblemDetails(Request.Path);
        }

        return Ok();
    }
}
