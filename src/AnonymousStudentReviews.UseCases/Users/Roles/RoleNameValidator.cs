using AnonymousStudentReviews.Core.Aggregates.Role;

namespace AnonymousStudentReviews.UseCases.Users.Roles;

public static class RoleNameValidator
{
    public static bool IsAllowed(string roleName)
    {
        return roleName == RoleNameConstants.Student
               || roleName == RoleNameConstants.Admin
               || roleName == RoleNameConstants.SuperAdmin;
    }
}
