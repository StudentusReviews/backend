namespace AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

public interface IApplicationRepository
{
    public Task<AppToAddAUni> Create(AppToAddAUni appToAddAUni);
    public Task DeleteByIdAsync(Guid id);
    public Task<List<AppToAddAUni>> GetAll();
    public Task<AppToAddAUni> GetByIdAsync(Guid id);
}
