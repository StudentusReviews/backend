using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Create;

public interface ICreateApplicationToAddAUniversityService
{
    Task<Result<Core.Aggregates.ApplicationToAddAUniversity.Base.ApplicationToAddAUniversity>> ExecuteAsync(CreateApplicationToAddAUniversityDto dto);
}
