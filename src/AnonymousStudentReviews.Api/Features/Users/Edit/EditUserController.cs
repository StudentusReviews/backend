using AnonymousStudentReviews.Api.Extensions;
using AnonymousStudentReviews.UseCases.Users.Edit;
using AnonymousStudentReviews.Core.Aggregates.Role;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnonymousStudentReviews.Api.Features.Users.Edit;

[Route("api/users/{userId:guid}")]
[Authorize(
    AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme,
    Roles = RoleNameConstants.AdminOrSuperAdmin
)]
[ApiController]
public class EditUserController : ControllerBase
{
    private readonly IEditUserService _editUserService;

    public EditUserController(IEditUserService editUserService)
    {
        _editUserService = editUserService;
    }

    [HttpPut]
    public async Task<ActionResult> EditUserAsync([FromRoute] Guid userId, [FromBody] EditUserRequest request)
    {
        var editUserResult = await _editUserService.HandleAsync(RequestToDto(userId, request));

        if (editUserResult.IsFailure)
        {
            return editUserResult.Error.ToProblemDetails(Request.Path);
        }

        return NoContent();
    }

    private EditUserDto RequestToDto(Guid userId, EditUserRequest request)
    {
        return new EditUserDto { IsBanned = request.IsBanned, UserId = userId };
    }
}
