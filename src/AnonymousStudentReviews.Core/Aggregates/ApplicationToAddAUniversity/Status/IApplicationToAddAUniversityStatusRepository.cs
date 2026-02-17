using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;

public interface IApplicationToAddAUniversityStatusRepository
{
    public Task<Result<ApplicationToAddAUniversityStatus>> GetStatusByNameAsync(string statusName);
}
