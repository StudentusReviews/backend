using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Users.Roles;

namespace AnonymousStudentReviews.UseCases.Users.Roles.Assign;

public class AssignUserRoleService : IAssignUserRoleService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignUserRoleService(
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

        if (user.Roles.Any(r => r.Name == role.Name))
        {
            return Result.Failure(UserRoleErrors.RoleAlreadyAssigned);
        }

        user.Roles.Add(role);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
