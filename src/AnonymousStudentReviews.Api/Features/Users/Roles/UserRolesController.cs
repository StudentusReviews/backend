using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.UseCases.Users.Roles.Assign;
using AnonymousStudentReviews.UseCases.Users.Roles.Remove;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Users.Roles;

[Route("api/users/{userId:guid}/roles")]
[Authorize(
    AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.SuperAdmin
)]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly IAssignUserRoleService _assignUserRoleService;
    private readonly IRemoveUserRoleService _removeUserRoleService;

    public UserRolesController(
        IAssignUserRoleService assignUserRoleService,
        IRemoveUserRoleService removeUserRoleService)
    {
        _assignUserRoleService = assignUserRoleService;
        _removeUserRoleService = removeUserRoleService;
    }

    [HttpPost]
    public async Task<ActionResult> AssignRoleAsync([FromRoute] Guid userId, [FromBody] AssignRoleRequest request)
    {
        var result = await _assignUserRoleService.HandleAsync(userId, request.RoleName);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return NoContent();
    }

    [HttpDelete("{roleName}")]
    public async Task<ActionResult> RemoveRoleAsync([FromRoute] Guid userId, [FromRoute] string roleName)
    {
        var result = await _removeUserRoleService.HandleAsync(userId, roleName);

        if (result.IsFailure)
        {
            return result.Error.ToProblemDetails(Request.Path);
        }

        return NoContent();
    }
}
