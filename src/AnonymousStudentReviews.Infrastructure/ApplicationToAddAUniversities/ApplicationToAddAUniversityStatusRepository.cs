using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Applications;

public class ApplicationToAddAUniversityStatusRepository : IApplicationToAddAUniversityStatusRepository
{
    private readonly ApplicationDbContext _context;

    public ApplicationToAddAUniversityStatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ApplicationToAddAUniversityStatus>> GetStatusByNameAsync(string statusName)
    {
        var status = await _context.ApplicationToAddAUniversityStatuses.FirstOrDefaultAsync(s => s.Name == statusName);
        if (status is null)
            return Result.Failure<ApplicationToAddAUniversityStatus>(ApplicationToAddAUniversityStatusErrors.StatusNotFound);
        return Result.Success(status);
    }
}
