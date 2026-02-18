using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.RetrieveOne;

public class RetrieveOneReviewService : IRetrieveOneReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public RetrieveOneReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<ReviewPreview>> HandleAsync(Guid reviewId)
    {
        var reviewResult = await _reviewRepository.GetPreviewByIdAsync(reviewId);

        if (reviewResult.IsFailure)
        {
            return Result.Failure<ReviewPreview>(reviewResult.Error);
        }

        return reviewResult.Value;
    }
}
