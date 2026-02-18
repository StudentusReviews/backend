using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.RetrieveMany;

public interface IRetrieveManyUniversitiesService
{
    Task<Result<OffsetPagedResult<UniversityPreview>>> HandleAsync(RetrieveManyUniversitiesDto dto);
}
