using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Delete;

public class DeleteApplicationToAddAUniversityService : IDeleteApplicationToAddAUniversityService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteApplicationToAddAUniversityService(IApplicationRepository applicationRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }
    public async Task<Result> ExecuteAsync(Guid appId)
    {
        var userId = _currentUser.UserId;
        if (userId == null) return Result.Failure(UserErrors.NotFound);
        await _applicationRepository.DeleteByIdAsync(appId);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
