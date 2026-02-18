namespace AnonymousStudentReviews.Core.Aggregates.Role;

public static class RoleNameConstants
{
    public const string Admin = "Admin";
    public const string Student = "Student";
    public const string SuperAdmin = "SuperAdmin";

    public const string AdminOrSuperAdmin = Admin + "," + SuperAdmin;
}
