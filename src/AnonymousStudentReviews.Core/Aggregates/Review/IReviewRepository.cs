using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.Review;

public interface IReviewRepository
{
    void Create(Review review);
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ReviewPreview>> GetPreviewByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid universityId, Guid userId, CancellationToken cancellationToken = default);
    void Delete(Review review);

    Task<CursorPagedResult<Review>> GetAllAsync(Guid? universityId, SortOrder sortOrder,
        ReviewCursor? cursor, int limit);
}
