using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.RetrieveMany;

public class RetrieveManyUniversitiesService : IRetrieveManyUniversitiesService
{
    private readonly IUniversityRepository _universityRepository;

    public RetrieveManyUniversitiesService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<Result<OffsetPagedResult<UniversityPreview>>> HandleAsync(RetrieveManyUniversitiesDto dto)
    {
        var result = await _universityRepository.GetAllOffsetAsync(
            dto.Query,
            dto.Name,
            dto.City,
            dto.UniversitySortBy,
            dto.SortOrder,
            dto.Offset,
            dto.Limit);

        return Result.Success(result);
    }
}
