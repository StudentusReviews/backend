namespace AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity;

public interface IApplicationRepository
{
    public Task<ApplicationToAddAUniversity> Create(ApplicationToAddAUniversity appToAddAUni);
    public Task DeleteByIdAsync(Guid id);
    public Task<List<ApplicationToAddAUniversity>> GetAll();
    public Task<ApplicationToAddAUniversity> GetByIdAsync(Guid id);
}
