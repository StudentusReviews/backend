using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Users.Roles;

namespace AnonymousStudentReviews.UseCases.Users.Roles.Remove;

public class RemoveUserRoleService : IRemoveUserRoleService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveUserRoleService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(Guid userId, string roleName)
    {
        if (!RoleNameValidator.IsAllowed(roleName))
        {
            return Result.Failure(UserRoleErrors.InvalidRoleName);
        }
        
        var userResult = await _userRepository.FindByIdAsync(userId);
        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Error);
        }

        var roleResult = await _roleRepository.GetRoleByNameAsync(roleName);
        if (roleResult.IsFailure)
        {
            return Result.Failure(roleResult.Error);
        }

        var user = userResult.Value;
        var role = roleResult.Value;

        user.Roles ??= new List<Role>(); 

        var roleToRemove = user.Roles.FirstOrDefault(r => r.Name == role.Name);
        if (roleToRemove is null)
        {
            return Result.Failure(UserRoleErrors.RoleNotAssigned);
        }

        user.Roles.Remove(roleToRemove);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
