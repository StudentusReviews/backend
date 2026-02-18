using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.University;

namespace AnonymousStudentReviews.UseCases.Universities.Delete;

public class DeleteUniversityService : IDeleteUniversityService
{
    private readonly IUniversityRepository _repository;

    public DeleteUniversityService(IUniversityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> ExecuteAsync(Guid universityId)
    {
        return await _repository.DeleteAsync(universityId);
    }
}
