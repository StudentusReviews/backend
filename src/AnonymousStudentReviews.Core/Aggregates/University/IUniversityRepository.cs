using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.University;

public interface IUniversityRepository
{
    Task<CursorPagedResult<UniversityPreview>> GetAllAsync(string? query, string? name, string? city, UniversitySortBy sortBy,
        SortOrder sortOrder,
        UniversityCursor? cursor, int limit);

    Task<Result<UniversityDetailedPreview>> FindByIdFetchDetailedPreviewAsync(Guid universityId);
}
