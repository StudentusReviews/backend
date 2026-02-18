using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.UploadIcon;

public class UploadUniversityIconService : IUploadUniversityIconService
{
    private readonly IUniversityRepository _repository;

    public UploadUniversityIconService(IUniversityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<string>> ExecuteAsync(Guid universityId, string iconUrl)
    {
        var result = await _repository.UpdateIconUrlAsync(universityId, iconUrl);

        if (result.IsFailure)
        {
            return Result.Failure<string>(result.Error);
        }

        return iconUrl;
    }
}
