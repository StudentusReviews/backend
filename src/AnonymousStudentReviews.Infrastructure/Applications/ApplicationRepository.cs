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

    public Task<AppToAddAUni> Create(AppToAddAUni appToAddAUni)
    {
        _dbContext.Applications.AddAsync(appToAddAUni);
        return Task.FromResult(appToAddAUni);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var application = await _dbContext.Applications.FindAsync(id);
        if (application is null)
            throw new InvalidOperationException($"Application with id {id} not found.");
        application.MarkAsDeleted();
        _dbContext.Applications.Update(application);
    }

    public Task<List<AppToAddAUni>> GetAll()
    {
        return Task.FromResult(_dbContext.Applications.Where(a => !a.IsDeleted).ToList());
    }

    public async Task<AppToAddAUni> GetByIdAsync(Guid id)
    {
        var application = await _dbContext.Applications.FindAsync(id);
        if (application is null || application.IsDeleted)
            throw new InvalidOperationException($"Application with id {id} not found.");
        return application;
    }
}
