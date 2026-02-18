using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Applications;

public class ApplicationToAddAUniversityRepository : IApplicationToAddAUniversityRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ApplicationToAddAUniversityRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(ApplicationToAddAUniversity applicationToAddAUniversity)
    {
        _dbContext.ApplicationToAddAUniversities.Add(applicationToAddAUniversity);
    }

    public async Task<Result> DeleteByIdAsync(Guid id)
    {
        var applicationToAddAUniversity = await GetByIdAsync(id);
        if (applicationToAddAUniversity.IsFailure)
            return Result.Failure(applicationToAddAUniversity.Error);
        applicationToAddAUniversity.Value.MarkAsDeleted();
        return Result.Success();
    }

    public async Task<List<ApplicationToAddAUniversity>> GetAll()
    {
        var applications = await _dbContext.ApplicationToAddAUniversities.Where(a => !a.IsDeleted).ToListAsync();
        return applications;
    }

    public async Task<Result<ApplicationToAddAUniversity>> GetByIdAsync(Guid id)
    {
        var application = await _dbContext.ApplicationToAddAUniversities.FindAsync(id);
        if (application is null || application.IsDeleted)
            return Result.Failure<ApplicationToAddAUniversity>(ApplicationToAddAUniversityErrors.ApplicationToAddAUniversityNotFound(id));
        return Result.Success(application);
    }
}
