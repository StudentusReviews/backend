using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Users.Roles.Assign;

public interface IAssignUserRoleService
{
    Task<Result> HandleAsync(Guid userId, string roleName);
}
