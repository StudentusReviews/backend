using System.Globalization;

using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.Infrastructure.Data;
using AnonymousStudentReviews.UseCases.Utils;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Universities;

public class UniversityRepository : IUniversityRepository
{
    private readonly ApplicationDatabaseContext _context;

    public UniversityRepository(ApplicationDatabaseContext context)
    {
        _context = context;
    }

    public async Task<CursorPagedResult<UniversityPreview>> GetAllAsync(string? query, string? name, string? city,
        UniversitySortBy universitySortBy, SortOrder sortOrder,
        UniversityCursor? cursor, int limit)
    {
        var dbQuery = _context.Universities.AsNoTracking();


        if (!string.IsNullOrWhiteSpace(query))
        {
            var searchTerm = query.Trim().ToLower();
            dbQuery = dbQuery.Where(u =>
                u.Name.ToLower().Contains(searchTerm) ||
                (u.City != null && u.City.ToLower().Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            dbQuery = dbQuery.Where(u => u.Name.ToLower().Contains(name.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            dbQuery = dbQuery.Where(u => u.City != null && u.City.ToLower().Contains(city.Trim()));
        }

        switch (universitySortBy)
        {
            case UniversitySortBy.Newest:
            default:
                if (cursor != null && DateTime.TryParse(cursor.Value, CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal, out var cursorDate))
                {
                    if (sortOrder == SortOrder.Descending)
                    {
                        dbQuery = dbQuery.Where(u =>
                            u.CreatedAt < cursorDate ||
                            (u.CreatedAt == cursorDate && u.Id > cursor.Id));
                    }
                    else
                    {
                        dbQuery = dbQuery.Where(u =>
                            u.CreatedAt > cursorDate ||
                            (u.CreatedAt == cursorDate && u.Id > cursor.Id));
                    }
                }

                dbQuery = sortOrder == SortOrder.Descending
                    ? dbQuery.OrderByDescending(u => u.CreatedAt).ThenBy(u => u.Id)
                    : dbQuery.OrderBy(u => u.CreatedAt).ThenBy(u => u.Id);
                break;

            case UniversitySortBy.Rating:
                if (cursor != null && double.TryParse(cursor.Value, NumberStyles.Float, CultureInfo.InvariantCulture,
                        out var cursorAverageScore))
                {
                    if (sortOrder == SortOrder.Descending)
                    {
                        dbQuery = dbQuery.Where(u =>
                            (u.Reviews.Any() ? u.Reviews.Average(e => e.Score) : 0) < cursorAverageScore ||
                            ((u.Reviews.Any() ? u.Reviews.Average(e => e.Score) : 0) == cursorAverageScore &&
                             u.Id > cursor.Id));
                    }
                    else
                    {
                        dbQuery = dbQuery.Where(u =>
                            (u.Reviews.Any() ? u.Reviews.Average(e => e.Score) : 0) > cursorAverageScore ||
                            ((u.Reviews.Any() ? u.Reviews.Average(e => e.Score) : 0) == cursorAverageScore &&
                             u.Id > cursor.Id));
                    }
                }

                dbQuery = sortOrder == SortOrder.Descending
                    ? dbQuery.OrderByDescending(u =>
                            u.Reviews.Any() ? u.Reviews.Average(e => e.Score) : 0)
                        .ThenBy(u => u.Id)
                    : dbQuery.OrderBy(u =>
                            u.Reviews.Any() ? u.Reviews.Average(e => e.Score) : 0)
                        .ThenBy(u => u.Id);
                break;

            case UniversitySortBy.ReviewCount:
                if (cursor != null && int.TryParse(cursor.Value, NumberStyles.Integer, CultureInfo.InvariantCulture,
                        out var cursorReviewCount))
                {
                    if (sortOrder == SortOrder.Descending)
                    {
                        dbQuery = dbQuery.Where(u =>
                            (u.Reviews.Any() ? u.Reviews.Count() : 0) < cursorReviewCount ||
                            ((u.Reviews.Any() ? u.Reviews.Count : 0) == cursorReviewCount &&
                             u.Id > cursor.Id));
                    }
                    else
                    {
                        dbQuery = dbQuery.Where(u =>
                            (u.Reviews.Any() ? u.Reviews.Count() : 0) > cursorReviewCount ||
                            ((u.Reviews.Any() ? u.Reviews.Count() : 0) == cursorReviewCount &&
                             u.Id > cursor.Id));
                    }
                }

                dbQuery = sortOrder == SortOrder.Descending
                    ? dbQuery.OrderByDescending(u =>
                            u.Reviews.Any() ? u.Reviews.Count() : 0)
                        .ThenBy(u => u.Id)
                    : dbQuery.OrderBy(u =>
                            u.Reviews.Any() ? u.Reviews.Count() : 0)
                        .ThenBy(u => u.Id);
                break;
        }

        var projectedQuery = dbQuery
            .Select(u => new UniversityPreview
            {
                Id = u.Id,
                Name = u.Name,
                City = u.City,
                Website = u.Website,
                CreatedAt = u.CreatedAt,
                AverageScore = u.Reviews.Any() ? u.Reviews.Average(e => e.Score) : 0,
                ReviewCount = u.Reviews.Any() ? u.Reviews.Count() : 0,
                IconUrl = u.IconUrl
            });


        var items = await projectedQuery.Take(limit + 1).ToListAsync();

        var hasNextPage = items.Count > limit;
        string? encodedNextCursor = null;

        if (hasNextPage)
        {
            items.RemoveAt(items.Count - 1);
            var lastItem = items.Last();

            var nextValue = universitySortBy switch
            {
                UniversitySortBy.Newest => lastItem.CreatedAt.ToString("O"),
                UniversitySortBy.Rating => lastItem.AverageScore.ToString(CultureInfo.InvariantCulture),
                UniversitySortBy.ReviewCount => lastItem.ReviewCount.ToString(),
                _ => string.Empty
            };

            var nextCursorObj = new UniversityCursor(nextValue, lastItem.Id);
            encodedNextCursor = CursorUtils.ToCursor(nextCursorObj);
        }

        return new CursorPagedResult<UniversityPreview>(items, encodedNextCursor, hasNextPage);
    }

    public async Task<Result<UniversityDetailedPreview>> FindByIdFetchDetailedPreviewAsync(Guid universityId)
    {
        var result = await _context.Universities
            .Select(e => new UniversityDetailedPreview
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                AverageScore = e.Reviews.Any() ? e.Reviews.Average(r => r.Score) : 0,
                ReviewCount = e.Reviews.Any() ? e.Reviews.Count() : 0,
                City = e.City,
                Website = e.Website,
                CreatedAt = e.CreatedAt,
                IconUrl = e.IconUrl
            }).FirstOrDefaultAsync(e => e.Id == universityId);

        if (result is null)
        {
            return Result.Failure<UniversityDetailedPreview>(UniversityErrors.NotFound);
        }

        return result;
    }
}
