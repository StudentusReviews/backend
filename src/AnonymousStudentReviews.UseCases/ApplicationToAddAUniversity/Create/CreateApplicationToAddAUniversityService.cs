using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;

public class CreateApplicationToAddAUniversityService : ICreateApplicationToAddAUniversityService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IApplicationToAddAUniversityRepository _applicationRepository;
    private readonly IApplicationToAddAUniversityStatusRepository _applicationStatusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManager _userManager;

    public CreateApplicationToAddAUniversityService(IApplicationToAddAUniversityRepository applicationRepository, IApplicationToAddAUniversityStatusRepository applicationStatusRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUser, IUserManager userManager)
    {
        _applicationRepository = applicationRepository;
        _applicationStatusRepository = applicationStatusRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _userManager = userManager;
    }

    public async Task<Result<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>> ExecuteAsync(CreateApplicationToAddAUniversityDto dto)
    {
        var userId = _currentUser.UserId;
        if (userId == null) return Result.Failure<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>(UserErrors.NotFound);

        var pendingStatusResult = await _applicationStatusRepository.GetStatusByNameAsync(StatusNameConstants.Pending);
        if (pendingStatusResult.IsFailure)
            throw new Exception("Pending status not found. This should never happen if the database is seeded correctly.");

        var appResult = Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity.Create(dto.UniversityName, dto.DomainName, dto.Comment, userId.Value, pendingStatusResult.Value.Id);

        if (appResult.IsFailure)
            return Result.Failure<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>(appResult.Error);

        await _applicationRepository.Create(appResult.Value);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(appResult.Value);
    }
}
