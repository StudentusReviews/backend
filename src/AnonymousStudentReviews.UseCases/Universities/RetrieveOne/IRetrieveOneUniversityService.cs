using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.RetrieveOne;

public interface IRetrieveOneUniversityService
{
    Task<Result<UniversityDetailedPreview>> HandleAsync(Guid universityId);
}
