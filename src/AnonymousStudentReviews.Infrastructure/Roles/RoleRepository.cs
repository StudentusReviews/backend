using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Roles;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Role>> GetRoleByNameAsync(string roleName)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(role => role.Name == roleName);

        if (role is null)
        {
            return Result.Failure<Role>(RoleErrors.RoleNotFound);
        }

        return Result.Success(role);
    }
}
