using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.AppToAddAUni.Create;

public class CreateAppToAddAUniService : ICreateAppToAddAUniService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManager _userManager;

    public CreateAppToAddAUniService(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUser, IUserManager userManager)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _userManager = userManager;
    }

    public async Task<Result<Core.Aggregates.AppToAddAUni.AppToAddAUni>> ExecuteAsync(CreateAppToAddAUniDto dto)
    {
        var userId = _currentUser.UserId;
        if (userId == null) return Result.Failure<Core.Aggregates.AppToAddAUni.AppToAddAUni>(UserErrors.NotFound);

        var appResult = Core.Aggregates.AppToAddAUni.AppToAddAUni.Create(dto.UniversityName, dto.DomainName, userId.Value);

        if (appResult.IsFailure)
            return Result.Failure<Core.Aggregates.AppToAddAUni.AppToAddAUni>(appResult.Error);

        _applicationRepository.Create(appResult.Value);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(appResult.Value);
    }
}
