using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.Infrastructure.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AnonymousStudentReviews.Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly LoginOptions _loginOptions;

    public UserRepository(ApplicationDbContext context, IOptions<LoginOptions> loginOptions)
    {
        _context = context;
        _loginOptions = loginOptions.Value;
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

    public async Task<Result<User>> FindByEmailHashAsync(string emailHash)
    {
        var result = await _context.Users
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.EmailHash == emailHash);

        if (result is null)
        {
            return Result.Failure<User>(UserErrors.NotFound);
        }

        return result;
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

    public bool IsUserEntityTracked(User user)
    {
        return _context.Entry(user).State != EntityState.Detached;
    }

    public void IncrementAccessFailedCount(User user)
    {
        user.AccessFailedCount++;
    }

    public void LockOutUser(User user)
    {
        user.LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(_loginOptions.LockoutTimeMinutes);
    }

    public void Ban(User user)
    {
        user.IsBanned = true;
    }

    public async Task<bool> UserHasRole(User user, Role role)
    {
        return await _context.Users
            .Include(e => e.Roles)
            .AnyAsync(e => e.Id == user.Id && e.Roles.Contains(role));
    }
}
