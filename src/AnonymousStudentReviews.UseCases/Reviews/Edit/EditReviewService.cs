using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

namespace AnonymousStudentReviews.UseCases.Reviews.Edit;

public class EditReviewService : IEditReviewService
{
    private readonly ICreateMessageInReviewOutboxService _createMessageInReviewOutboxService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService, ICreateMessageInReviewOutboxService createMessageInReviewOutboxService)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _createMessageInReviewOutboxService = createMessageInReviewOutboxService;
    }

    public async Task<Result<Review>> ExecuteAsync(EditReviewDto dto)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            throw new InvalidOperationException("Authentication system is misbehaving");
        }

        var review = await _reviewRepository.GetByIdAsync(dto.ReviewId);

        if (review is null)
        {
            return Result.Failure<Review>(ReviewErrors.NotFound);
        }

        if (review.UserId != userId.Value)
        {
            return Result.Failure<Review>(ReviewErrors.AccessDenied);
        }

        var updateResult = review.Update(dto.Score, dto.Body);

        if (updateResult.IsFailure)
        {
            return Result.Failure<Review>(updateResult.Error);
        }

        await _unitOfWork.SaveChangesAsync();


        await _createMessageInReviewOutboxService.HandleAsync(new CreateMessageInReviewOutboxDto
        {
            Review = review,
            ReviewOutboxState = ReviewOutboxState.PendingUpdate
        });
        
        return Result.Success(review);
    }
}
