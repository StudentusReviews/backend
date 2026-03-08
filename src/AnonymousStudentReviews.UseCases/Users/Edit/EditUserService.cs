using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Abstractions;

namespace AnonymousStudentReviews.UseCases.Users.Edit;

public class EditUserService : IEditUserService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditUserService(IUserRepository userRepository, ICurrentUserService currentUserService,
        IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(EditUserDto dto)
    {
        var getUserToBeEditedResult = await _userRepository.FindByIdAsync(dto.UserId);

        if (getUserToBeEditedResult.IsFailure)
        {
            return Result.Failure(EditUserErrors.NotFound);
        }

        var userToBeEdited = getUserToBeEditedResult.Value;

        if (_currentUserService.UserId is null)
        {
            throw new Exception("Current user id is null, authentication is broken");
        }

        var currentUserId = _currentUserService.UserId ?? Guid.Empty;

        var currentUserResult = await _userRepository.FindByIdAsync(currentUserId);

        if (currentUserResult.IsFailure)
        {
            throw new Exception("Current user id is not present in the db");
        }

        var currentUser = currentUserResult.Value;

        var superAdminRole = await _roleRepository.GetRoleByNameAsync(RoleNameConstants.SuperAdmin);
        var adminRole = await _roleRepository.GetRoleByNameAsync(RoleNameConstants.Admin);

        if (superAdminRole.IsFailure || adminRole.IsFailure)
        {
            throw new Exception("Roles are not present in the db, seeding issues");
        }

        var currentUserIsSuperAdmin = await _userRepository.UserHasRoleAsync(currentUser, superAdminRole.Value);
        var currentUserIsAdmin = await _userRepository.UserHasRoleAsync(currentUser, adminRole.Value);

        var userToBeEditedIsSuperAdmin = await _userRepository.UserHasRoleAsync(userToBeEdited, superAdminRole.Value);
        var userToBeEditedIsAdmin = await _userRepository.UserHasRoleAsync(userToBeEdited, adminRole.Value);

        if ((currentUserIsAdmin && (userToBeEditedIsAdmin || userToBeEditedIsSuperAdmin)) ||
            (currentUserIsSuperAdmin && userToBeEditedIsSuperAdmin))
        {
            return Result.Failure(EditUserErrors.AccessDenied);
        }

        if (userToBeEdited.IsBanned == dto.IsBanned)
        {
            return Result.Failure(EditUserErrors.NoChangedDetected);
        }

        userToBeEdited.IsBanned = dto.IsBanned;
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
