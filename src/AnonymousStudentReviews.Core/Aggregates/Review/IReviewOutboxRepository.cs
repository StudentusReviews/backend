namespace AnonymousStudentReviews.Core.Aggregates.Review;

public interface IReviewOutboxRepository
{
    void Create(ReviewOutbox reviewOutbox);
    Task<ReviewOutboxStateEntity> GetReviewOutboxStateEntityAsync(ReviewOutboxState state);
    Task<bool> ReviewOutboxExistsAsync(ReviewOutboxStateEntity state, Review review);
}
