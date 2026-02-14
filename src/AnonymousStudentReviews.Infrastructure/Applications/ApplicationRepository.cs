using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity;
using AnonymousStudentReviews.Infrastructure.Data;

namespace AnonymousStudentReviews.Infrastructure.Applications;

public class ApplicationRepository: IApplicationRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ApplicationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ApplicationToAddAUniversity> Create(ApplicationToAddAUniversity appToAddAUni)
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

    public Task<List<ApplicationToAddAUniversity>> GetAll()
    {
        return Task.FromResult(_dbContext.Applications.Where(a => !a.IsDeleted).ToList());
    }

    public async Task<ApplicationToAddAUniversity> GetByIdAsync(Guid id)
    {
        var application = await _dbContext.Applications.FindAsync(id);
        if (application is null || application.IsDeleted)
            throw new InvalidOperationException($"Application with id {id} not found.");
        return application;
    }
}
