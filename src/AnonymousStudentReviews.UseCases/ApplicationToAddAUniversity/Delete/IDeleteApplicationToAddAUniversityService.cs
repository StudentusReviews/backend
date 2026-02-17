using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.ApplicationToAddAUniversity.Delete;

public interface IDeleteApplicationToAddAUniversityService
{
    public Task<Result> ExecuteAsync(Guid applicationToAddAUniversityId);
}
