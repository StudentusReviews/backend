using AnonymousStudentReviews.Core.Aggregates.EmailVerificationToken;
using AnonymousStudentReviews.Infrastructure.Data;

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
}
