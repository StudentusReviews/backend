using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

public interface IApplicationToAddAUniversityRepository
{
    public void Create(ApplicationToAddAUniversity applicationToAddAUniversity);
    public Task<Result> DeleteByIdAsync(Guid id);
    public Task<List<ApplicationToAddAUniversity>> GetAll();
    public Task<Result<ApplicationToAddAUniversity>> GetByIdAsync(Guid id);
}
