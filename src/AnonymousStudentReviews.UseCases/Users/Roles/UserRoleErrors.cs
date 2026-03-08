using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.UseCases.Users.Roles;

public static class UserRoleErrors
{
    public static readonly NotFoundError InvalidRoleName =
        new("UserRole.InvalidRoleName", "Invalid role name");

    public static readonly AlreadyExistsError RoleAlreadyAssigned =
        new("UserRole.RoleAlreadyAssigned", "Role is already assigned to the user");

    public static readonly NotFoundError RoleNotAssigned =
        new("UserRole.RoleNotAssigned", "Role is not assigned to the user");
}
