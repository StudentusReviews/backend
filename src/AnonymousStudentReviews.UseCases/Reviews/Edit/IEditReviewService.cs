using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.Edit;

public interface IEditReviewService
{
    Task<Result<Review>> ExecuteAsync(EditReviewDto dto);
}
