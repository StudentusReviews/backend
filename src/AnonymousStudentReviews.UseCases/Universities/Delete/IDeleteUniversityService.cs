using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.UseCases.Universities.Delete;

public interface IDeleteUniversityService
{
    Task<Result> ExecuteAsync(Guid universityId);
}
