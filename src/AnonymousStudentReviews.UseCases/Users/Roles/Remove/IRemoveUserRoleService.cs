using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Users.Roles.Remove;

public interface IRemoveUserRoleService
{
    Task<Result> HandleAsync(Guid userId, string roleName);
}
