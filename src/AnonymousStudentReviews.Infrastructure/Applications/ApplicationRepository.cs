using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;
using AnonymousStudentReviews.Infrastructure.Data;

namespace AnonymousStudentReviews.Infrastructure.Applications;

public class ApplicationRepository: IApplicationRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ApplicationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ApplicationToAddAUniversity>> Create(ApplicationToAddAUniversity appToAddAUni)
    {
        await _dbContext.Applications.AddAsync(appToAddAUni);
        return Result.Success(appToAddAUni);
    }

    public async Task<Result> DeleteByIdAsync(Guid id)
    {
        var application = await _dbContext.Applications.FindAsync(id);
        if (application is null)
            return Result.Failure(new("ApplicationToAddAUniversity.ApplicationNotFound", $"Application with id {id} not found."));
        application.MarkAsDeleted();
        _dbContext.Applications.Update(application);
        return Result.Success();
    }

    public async Task<Result<List<ApplicationToAddAUniversity>>> GetAll()
    {
        return Result.Success<List<ApplicationToAddAUniversity>>(_dbContext.Applications.Where(a => !a.IsDeleted).ToList());
    }

    public async Task<Result<ApplicationToAddAUniversity>> GetByIdAsync(Guid id)
    {
        var application = await _dbContext.Applications.FindAsync(id);
        if (application is null || application.IsDeleted)
            return Result.Failure<ApplicationToAddAUniversity>(new("ApplicationToAddAUniversity.ApplicationNotFound", $"Application with id {id} not found."));
        return Result.Success(application);
    }
}
