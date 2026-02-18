using System.Globalization;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.UseCases.Utils;

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

    public async Task<CursorPagedResult<Review>> GetAllAsync(Guid? universityId, SortOrder sortOrder,
        ReviewCursor? cursor,
        int limit)
    {
        var dbQuery = _databaseContext.Reviews.AsNoTracking();

        if (universityId is not null && universityId != Guid.Empty)
        {
            dbQuery = dbQuery.Where(e => e.UniversityId == universityId);
        }

        if (cursor is not null && DateTime.TryParse(cursor.Value, CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal, out var cursorDate))
        {
            if (sortOrder == SortOrder.Descending)
            {
                dbQuery = dbQuery.Where(e =>
                    e.CreatedAt < cursorDate || (e.CreatedAt == cursorDate && e.Id > cursor.Id));
            }
            else
            {
                dbQuery = dbQuery.Where(e =>
                    e.CreatedAt > cursorDate || (e.CreatedAt == cursorDate && e.Id > cursor.Id));
            }
        }


        dbQuery = sortOrder == SortOrder.Descending
            ? dbQuery.OrderByDescending(e => e.CreatedAt)
                .ThenBy(e => e.Id)
            : dbQuery.OrderBy(e => e.CreatedAt)
                .ThenBy(e => e.Id);

        var items = await dbQuery.Take(limit + 1).ToListAsync();

        var hasNextPage = items.Count > limit;
        string? encodedNextCursor = null;

        if (hasNextPage)
        {
            items.RemoveAt(items.Count - 1);
            var lastItem = items.Last();

            var nextValue = lastItem.CreatedAt.ToString("O");

            var nextCursorObj = new ReviewCursor(nextValue, lastItem.Id);
            encodedNextCursor = CursorUtils.ToCursor(nextCursorObj);
        }

        return new CursorPagedResult<Review>(items, encodedNextCursor, hasNextPage);
    }
}
