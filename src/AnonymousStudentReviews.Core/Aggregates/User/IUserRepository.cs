using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.User;

public interface IUserRepository
{
    Task<bool> UserWithEmailHashExistsAsync(string emailHash);
    void CreateUser(User user);
    void ConfirmUser(User user);
    Task<Result<User>> FindByIdAsync(Guid id);
    Task<Result<User>> FindByEmailHashAsync(string email);
    Task<IEnumerable<Role.Role>> GetRolesAsync(User user);
    bool IsUserEntityTracked(User user);
    void IncrementAccessFailedCount(User user);
    void LockOutUser(User user);
    void Ban(User user);
    Task<bool> UserHasRoleAsync(User user, Role.Role role);
    Task<ICollection<Role.Role>> GetUserRoles(User user);

    Task<PagedResponse<UserPreview>> GetAllAsync(string? queryString = null, Guid? userId = null,
        Guid? universityId = null,
        string? universityName = null,
        string? emailHash = null, SortBy sortBy = SortBy.UniversityName, SortOrder sortOrder = SortOrder.Ascending,
        int pageNumber = 1, int pageSize = 10);
}
