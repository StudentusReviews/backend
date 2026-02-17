using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;
using AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

namespace AnonymousStudentReviews.UseCases.Reviews.Create;

public class CreateReviewService : ICreateReviewService
{
    private readonly ICreateMessageInReviewOutboxService _createMessageInReviewOutboxService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserManager _userManager;

    public CreateReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IUserManager userManager, ICreateMessageInReviewOutboxService createMessageInReviewOutboxService)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _createMessageInReviewOutboxService = createMessageInReviewOutboxService;
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

        var alreadyExists = await _reviewRepository.ExistsAsync(dto.UniversityId, userId.Value);

        if (alreadyExists)
        {
            return Result.Failure<Review>(ReviewErrors.AlreadyExists);
        }

        var createReviewResult = Review.Create(dto.UniversityId, userId.Value, dto.Score, dto.Body);

        if (createReviewResult.IsFailure)
        {
            return createReviewResult;
        }

        _reviewRepository.Create(createReviewResult.Value);
        await _unitOfWork.SaveChangesAsync();

        await _createMessageInReviewOutboxService.HandleAsync(new CreateMessageInReviewOutboxDto
        {
            Review = createReviewResult.Value, ReviewOutboxState = ReviewOutboxState.PendingAdd
        });

        return Result.Success(createReviewResult.Value);
    }
}
