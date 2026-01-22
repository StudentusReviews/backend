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
}
