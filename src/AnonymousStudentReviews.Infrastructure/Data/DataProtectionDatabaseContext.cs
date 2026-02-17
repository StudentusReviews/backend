using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Data;

public class DataProtectionDatabaseContext : DbContext, IDataProtectionKeyContext
{
    public DataProtectionDatabaseContext(DbContextOptions<DataProtectionDatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
}
