namespace AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

public interface IApplicationRepository
{
    public void Create(AppToAddAUni appToAddAUni);
    public Task DeleteByIdAsync(Guid id);
}
