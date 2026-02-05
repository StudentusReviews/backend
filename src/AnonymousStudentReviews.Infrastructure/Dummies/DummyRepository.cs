using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Infrastructure.Data;

namespace AnonymousStudentReviews.Infrastructure.Dummies;

public class DummyRepository : IDummyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DummyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(Dummy dummy)
    {
        _dbContext.Dummies.Add(dummy);
    }
}
