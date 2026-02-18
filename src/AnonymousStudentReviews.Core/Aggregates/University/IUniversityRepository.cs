using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.University;

public interface IUniversityRepository
{
    Task<CursorPagedResult<UniversityPreview>> GetAllAsync(string? query, string? name, string? city, UniversitySortBy sortBy,
        SortOrder sortOrder,
        UniversityCursor? cursor, int limit);

    Task<OffsetPagedResult<UniversityPreview>> GetAllOffsetAsync(
        string? query, string? name, string? city,
        UniversitySortBy sortBy,
        SortOrder sortOrder,
        int offset, int limit);

    Task<Result<UniversityDetailedPreview>> FindByIdFetchDetailedPreviewAsync(Guid universityId);

    Task AddAsync(University university);

    Task<Result<University>> UpdateAsync(University university);

    Task<Result> DeleteAsync(Guid universityId);

    Task<Result<University>> FindByIdAsync(Guid universityId);

    Task<Result<University>> UpdateIconUrlAsync(Guid universityId, string iconUrl);
}
