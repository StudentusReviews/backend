using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.Create;

public interface ICreateUniversityService
{
    Task<Result<University>> ExecuteAsync(CreateUniversityDto dto);
}
