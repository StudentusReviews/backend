using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Abstractions;

namespace AnonymousStudentReviews.UseCases.Reviews.Delete;

public class DeleteReviewService : IDeleteReviewService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
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

        _reviewRepository.Delete(review);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
