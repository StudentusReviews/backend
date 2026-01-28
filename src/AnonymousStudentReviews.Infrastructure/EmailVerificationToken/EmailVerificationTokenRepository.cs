using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.EmailVerificationToken;

public class EmailVerificationTokenRepository : IEmailVerificationTokenRepository
{
    private readonly ApplicationDbContext _context;

    public EmailVerificationTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Create(Core.Aggregates.EmailVerificationToken.EmailVerificationToken emailVerificationToken)
    {
        _context.EmailVerificationTokens.Add(emailVerificationToken);
    }

    public async Task<Result<Core.Aggregates.EmailVerificationToken.EmailVerificationToken>> GetByTokenHash(
        string tokenHash)
    {
        var result = await _context.EmailVerificationTokens
            .FirstOrDefaultAsync(e => e.TokenHash == tokenHash);

        if (result is null)
        {
            return Result.Failure<Core.Aggregates.EmailVerificationToken.EmailVerificationToken>(
                EmailVerificationTokenErrors.NotFound);
        }

        return Result.Success(result);
    }

    public void RedeemToken(Core.Aggregates.EmailVerificationToken.EmailVerificationToken token)
    {
        token.UsedAt = DateTime.UtcNow;
    }
}
