using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.RetrieveOne;

public class RetrieveOneUniversityService : IRetrieveOneUniversityService
{
    private readonly IUniversityRepository _universityRepository;

    public RetrieveOneUniversityService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<Result<UniversityDetailedPreview>> HandleAsync(Guid universityId)
    {
        var result = await _universityRepository.FindByIdFetchDetailedPreviewAsync(universityId);

        return result;
    }
}
