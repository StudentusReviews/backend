using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.Update;

public interface IUpdateUniversityService
{
    Task<Result<University>> ExecuteAsync(UpdateUniversityDto dto);
}
