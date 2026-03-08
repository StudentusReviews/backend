using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.UseCases.Abstractions;

namespace AnonymousStudentReviews.UseCases.Reviews.Edit;

public class EditReviewService : IEditReviewService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
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


        return Result.Success(review);
    }
}
