using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

public interface IApplicationRepository
{
    public Task<Result<ApplicationToAddAUniversity>> Create(ApplicationToAddAUniversity appToAddAUni);
    public Task<Result> DeleteByIdAsync(Guid id);
    public Task<Result<List<ApplicationToAddAUniversity>>> GetAll();
    public Task<Result<ApplicationToAddAUniversity>> GetByIdAsync(Guid id);
}
