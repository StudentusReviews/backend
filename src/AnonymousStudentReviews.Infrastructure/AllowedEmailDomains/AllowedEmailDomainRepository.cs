using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.AllowedEmailDomains;

public class AllowedEmailDomainRepository : IAllowedEmailDomainRepository
{
    private readonly ApplicationDbContext _context;

    public AllowedEmailDomainRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AllowedEmailDomain>> FindByDomainAsync(string domain)
    {
        var result = await _context.AllowedEmailDomains
            .FirstOrDefaultAsync(e => e.Domain == domain);

        if (result is null)
        {
            return Result.Failure<AllowedEmailDomain>(AllowedEmailDomainErrors.NotFound);
        }

        return Result.Success(result);
    }

    public async Task<bool> IsEmailDomainAllowed(string emailDomain)
    {
        return await _context.AllowedEmailDomains.AnyAsync(domain => domain.Domain == emailDomain);
    }
}
