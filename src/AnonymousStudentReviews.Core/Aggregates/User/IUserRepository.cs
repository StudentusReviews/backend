using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.User;

public interface IUserRepository
{
    Task<bool> UserWithEmailHashExistsAsync(string emailHash);
    void CreateUser(User user);
    void ConfirmUser(User user);
    Task<Result<User>> FindByIdAsync(Guid id);
    Task<IEnumerable<Role.Role>> GetRolesAsync(User user);
}
