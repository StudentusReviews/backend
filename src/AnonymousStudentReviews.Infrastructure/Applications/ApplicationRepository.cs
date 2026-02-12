using AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;
using AnonymousStudentReviews.Infrastructure.Data;

namespace AnonymousStudentReviews.Infrastructure.Applications;

public class ApplicationRepository: IApplicationRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ApplicationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(AppToAddAUni appToAddAUni)
    {
        _dbContext.Applications.Add(appToAddAUni);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var application = await _dbContext.Applications.FindAsync(id);
        if (application != null)
        {
            application.MarkAsDeleted();
            _dbContext.Applications.Update(application);
        }
    }
}
