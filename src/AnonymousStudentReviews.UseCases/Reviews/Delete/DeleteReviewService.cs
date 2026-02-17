using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Reviews.Outbox.CreateMessage;

namespace AnonymousStudentReviews.UseCases.Reviews.Delete;

public class DeleteReviewService : IDeleteReviewService
{
    private readonly ICreateMessageInReviewOutboxService _createMessageInReviewOutboxService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService, ICreateMessageInReviewOutboxService createMessageInReviewOutboxService)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _createMessageInReviewOutboxService = createMessageInReviewOutboxService;
    }

    public async Task<Result> ExecuteAsync(DeleteReviewDto dto)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
        {
            throw new InvalidOperationException("Authentication system is misbehaving");
        }

        var review = await _reviewRepository.GetByIdAsync(dto.ReviewId);

        if (review is null)
        {
            return Result.Failure(ReviewErrors.NotFound);
        }

        if (review.UserId != userId.Value)
        {
            return Result.Failure(ReviewErrors.AccessDenied);
        }

        await _createMessageInReviewOutboxService.HandleAsync(new CreateMessageInReviewOutboxDto
        {
            Score = review.Score, ReviewOutboxAction = ReviewOutboxAction.Delete
        });

        _reviewRepository.Delete(review);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
