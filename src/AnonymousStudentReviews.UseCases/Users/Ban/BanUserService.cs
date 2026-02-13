using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Abstractions;

namespace AnonymousStudentReviews.UseCases.Users.Ban;

public class BanUserService : IBanUserService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public BanUserService(IUnitOfWork unitOfWork, IUserRepository userRepository,
        ICurrentUserService currentUserService, IRoleRepository roleRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _roleRepository = roleRepository;
    }

    public async Task<Result> HandleAsync(Guid userId)
    {
        var currentUserId = _currentUserService.UserId ?? Guid.Empty;
        var currentUserResult = await _userRepository.FindByIdAsync(currentUserId);

        if (currentUserResult.IsFailure)
        {
            throw new InvalidOperationException("Authenticated user does not exist");
        }

        if (currentUserId == userId)
        {
            return Result.Failure(BanUserErrors.SelfBan);
        }

        var userToBeBannedResult = await _userRepository.FindByIdAsync(userId);

        if (userToBeBannedResult.IsFailure)
        {
            return Result.Failure(BanUserErrors.UserNotFound);
        }

        var userToBeBanned = userToBeBannedResult.Value;

        var adminRoleResult = await _roleRepository.GetRoleByNameAsync(RoleNameConstants.Admin);

        if (adminRoleResult.IsFailure)
        {
            throw new InvalidOperationException("Admin role not found, likely an issue with seeding");
        }

        var adminRole = adminRoleResult.Value;

        var userToBeBannedIsAdmin = await _userRepository.UserHasRoleAsync(userToBeBanned, adminRole);

        if (userToBeBannedIsAdmin)
        {
            return Result.Failure(BanUserErrors.UserIsAdmin);
        }

        _userRepository.Ban(userToBeBanned);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
