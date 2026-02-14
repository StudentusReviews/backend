using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

namespace AnonymousStudentReviews.UseCases.Reviews.Create;

public class CreateReviewService : ICreateReviewService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManager _userManager;

    public CreateReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IUserManager userManager)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<Result<Review>> ExecuteAsync(CreateReviewDto dto)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            throw new InvalidOperationException("Authentication system is misbehaving");
        }

        var getUserResult = await _userManager.FindByIdAsync(userId.Value);

        if (getUserResult.IsFailure)
        {
            return Result.Failure<Review>(getUserResult.Error);
        }

        var user = getUserResult.Value;

        if (user.UniversityId is null)
        {
            return Result.Failure<Review>(ReviewErrors.UniversityNotSet);
        }

        if (dto.UniversityId != user.UniversityId.Value)
        {
            return Result.Failure<Review>(ReviewErrors.UniversityMismatch);
        }

        var createReviewResult = Review.Create(dto.UniversityId, userId.Value, dto.Score, dto.Body);

        if (createReviewResult.IsFailure)
        {
            return createReviewResult;
        }

        _reviewRepository.Create(createReviewResult.Value);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(createReviewResult.Value);
    }
}
