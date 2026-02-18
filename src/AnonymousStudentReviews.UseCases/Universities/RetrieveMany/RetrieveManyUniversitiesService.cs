using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.UseCases.Utils;

namespace AnonymousStudentReviews.UseCases.Universities.RetrieveMany;

public class RetrieveManyUniversitiesService : IRetrieveManyUniversitiesService
{
    private readonly IUniversityRepository _universityRepository;

    public RetrieveManyUniversitiesService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<Result<CursorPagedResult<UniversityPreview>>> HandleAsync(RetrieveManyUniversitiesDto dto)
    {
        var cursor = CursorUtils.FromCursor<UniversityCursor>(dto.Cursor);

        var result =
            await _universityRepository.GetAllAsync(dto.Query, dto.Name, dto.City, dto.SortBy,
                dto.SortOrder, cursor, dto.Limit);


        return Result.Success(result);
    }
}
