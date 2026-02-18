using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Delete;

public class DeleteApplicationToAddAUniversityService : IDeleteApplicationToAddAUniversityService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IApplicationToAddAUniversityRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteApplicationToAddAUniversityService(IApplicationToAddAUniversityRepository applicationRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> ExecuteAsync(Guid applicationToAddAUniversityId)
    {
        var userId = _currentUser.UserId;
        if (userId == null)
            return Result.Failure(UserErrors.NotFound);

        var application = await _applicationRepository.GetByIdAsync(applicationToAddAUniversityId);
        if (application == null)
            return Result.Failure(ApplicationToAddAUniversityErrors.ApplicationToAddAUniversityNotFound(applicationToAddAUniversityId));

        if (application.Value.UserId != userId)
            return Result.Failure(ApplicationToAddAUniversityErrors.ApplicationToAddAUniversityInaccessible(applicationToAddAUniversityId, (Guid)userId));

        await _applicationRepository.DeleteByIdAsync(applicationToAddAUniversityId);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
