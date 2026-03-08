using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Reviews.Delete;

public interface IDeleteReviewService
{
    Task<Result> ExecuteAsync(DeleteReviewDto dto);
}
