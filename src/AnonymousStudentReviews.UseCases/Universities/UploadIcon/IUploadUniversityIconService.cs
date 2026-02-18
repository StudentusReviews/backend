using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Universities.UploadIcon;

public interface IUploadUniversityIconService
{
    Task<Result<string>> ExecuteAsync(Guid universityId, string iconUrl);
}
