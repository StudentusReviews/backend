using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Reviews;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDatabaseContext _databaseContext;

    public ReviewRepository(ApplicationDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public void Create(Review review)
    {
        _databaseContext.Set<Review>().Add(review);
    }

    public async Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Set<Review>()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid universityId, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.Set<Review>()
            .AnyAsync(r => r.UniversityId == universityId && r.UserId == userId, cancellationToken);
    }

    public void Delete(Review review)
    {
        _databaseContext.Set<Review>().Remove(review);
    }
}
