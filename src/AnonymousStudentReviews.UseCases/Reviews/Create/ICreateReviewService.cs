using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;

namespace AnonymousStudentReviews.UseCases.Reviews.Create;

public interface ICreateReviewService
{
    Task<Result<Review>> ExecuteAsync(CreateReviewDto dto);
}
