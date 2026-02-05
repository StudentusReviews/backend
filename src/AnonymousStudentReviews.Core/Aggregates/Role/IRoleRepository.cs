using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.Role;

public interface IRoleRepository
{
    Task<Result<Role>> GetRoleByNameAsync(string roleName);
}
