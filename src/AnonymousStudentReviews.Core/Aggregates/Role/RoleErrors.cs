using AnonymousStudentReviews.Core.ErrorTypes;

namespace AnonymousStudentReviews.Core.Aggregates.Role;

public static class RoleErrors
{
    public static readonly NotFoundError RoleNotFound = new("Role.NotFound", "Role not found");
}
