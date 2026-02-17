using AnonymousStudentReviews.Core.Abstractions;

namespace AnonymousStudentReviews.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDatabaseContext _databaseContext;

    public UnitOfWork(ApplicationDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _databaseContext.SaveChangesAsync(cancellationToken);
    }
}
