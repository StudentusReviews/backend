using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.RetrieveMany;

public interface IRetrieveManyReviewsService
{
    Task<CursorPagedResult<Review>> HandleAsync(RetrieveManyReviewsDto dto);
}
