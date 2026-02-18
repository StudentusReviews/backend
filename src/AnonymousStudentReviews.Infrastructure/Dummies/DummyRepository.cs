using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Infrastructure.Data;

namespace AnonymousStudentReviews.Infrastructure.Dummies;

public class DummyRepository : IDummyRepository
{
    private readonly ApplicationDatabaseContext _databaseContext;

    public DummyRepository(ApplicationDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public void Create(Dummy dummy)
    {
        _databaseContext.Dummies.Add(dummy);
    }
}
