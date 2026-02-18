using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.Retrieve;

public interface IRetrieveManyReviewsService
{
    Task<CursorPagedResult<Review>> HandleAsync(RetrieveManyReviewsDto dto);
}
