using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;
using AnonymousStudentReviews.Infrastructure.Data;

namespace AnonymousStudentReviews.Infrastructure.Applications;

public class ApplicationStatusRepository : IApplicationStatusRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicationStatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ApplicationToAddAUniversityStatus>> GetStatusByNameAsync(string statusName)
    {         var status = _context.ApplicationStatuses.FirstOrDefault(s => s.Name == statusName);
        if (status is null)
            return Result.Failure<ApplicationToAddAUniversityStatus>(ApplicationToAddAUniversityStatusErrors.StatusNotFound);
        return Result.Success(status);
    }
}
