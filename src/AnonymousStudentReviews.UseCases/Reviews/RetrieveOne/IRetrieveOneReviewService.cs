using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.RetrieveOne;

public interface IRetrieveOneReviewService
{
    Task<Result<ReviewPreview>> HandleAsync(Guid reviewId);
}
