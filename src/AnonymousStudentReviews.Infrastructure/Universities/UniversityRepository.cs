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
        SortBy sortBy, SortOrder sortOrder,
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

        switch (sortBy)
        {
            case SortBy.Newest:
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

            case SortBy.Rating:
                throw new NotImplementedException("Rating sort not yet implemented.");

            case SortBy.ReviewCount:
                throw new NotImplementedException("Review count sort not yet implemented.");
        }


        var projectedQuery = dbQuery.Select(u => new UniversityPreview
        {
            Id = u.Id,
            Name = u.Name,
            City = u.City,
            Website = u.Website,
            CreatedAt = u.CreatedAt
        });


        var items = await projectedQuery.Take(limit + 1).ToListAsync();

        var hasNextPage = items.Count > limit;
        string? encodedNextCursor = null;

        if (hasNextPage)
        {
            items.RemoveAt(items.Count - 1);
            var lastItem = items.Last();

            var nextValue = sortBy switch
            {
                SortBy.Newest => lastItem.CreatedAt.ToString("O"),
                _ => string.Empty
            };

            var nextCursorObj = new UniversityCursor(nextValue, lastItem.Id);
            encodedNextCursor = CursorUtils.ToCursor(nextCursorObj);
        }

        return new CursorPagedResult<UniversityPreview>(items, encodedNextCursor, hasNextPage);
    }
}
