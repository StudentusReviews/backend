namespace AnonymousStudentReviews.Core.Aggregates.Review;

public interface IReviewRepository
{
    void Create(Review review);
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid universityId, Guid userId, CancellationToken cancellationToken = default);
    void Delete(Review review);
}
