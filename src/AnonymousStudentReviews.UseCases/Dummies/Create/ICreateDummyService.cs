using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Dummy;

namespace AnonymousStudentReviews.UseCases.Dummies.Create;

public interface ICreateDummyService
{
    Task<Result<Dummy>> ExecuteAsync(CreateDummyDto dto);
}
