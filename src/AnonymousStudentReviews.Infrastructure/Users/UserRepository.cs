using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserWithEmailHashExistsAsync(string emailHash)
    {
        return await _context.Users.AnyAsync(user => user.EmailHash == emailHash);
    }

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
    }

    public void ConfirmUser(User user)
    {
        user.EmailConfirmed = true;
    }

    public async Task<Result<User>> FindByIdAsync(Guid id)
    {
        var result = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (result is null)
        {
            return Result.Failure<User>(UserErrors.NotFound);
        }

        return Result.Success(result);
    }

    public async Task<IEnumerable<Role>> GetRolesAsync(User user)
    {
        var roles = await _context.Users
            .Include(queryUser => queryUser.Roles)
            .Where(queryUser => queryUser.Id == user.Id)
            .Select(queryUser => queryUser.Roles)
            .FirstOrDefaultAsync();

        return roles ?? [];
    }
}
