using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;

public class CreateApplicationToAddAUniversityService : ICreateApplicationToAddAUniversityService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManager _userManager;

    public CreateApplicationToAddAUniversityService(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUser, IUserManager userManager)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _userManager = userManager;
    }

    public async Task<Result<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>> ExecuteAsync(CreateApplicationToAddAUniversityDto dto)
    {
        var userId = _currentUser.UserId;
        if (userId == null) return Result.Failure<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>(UserErrors.NotFound);

        var appResult = Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity.Create(dto.UniversityName, dto.DomainName, userId.Value);

        if (appResult.IsFailure)
            return Result.Failure<Core.Aggregates.ApplicationToAddAUniversity.ApplicationToAddAUniversity>(appResult.Error);

        _applicationRepository.Create(appResult.Value);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(appResult.Value);
    }
}
