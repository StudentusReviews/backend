using AnonymousStudentReviews.Core;
using AnonymousStudentReviews.Core.DummyAggregate;
using AnonymousStudentReviews.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    protected ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Dummy> Dummies { get; set; }
    public DbSet<University> Universities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DummyConfiguration).Assembly);
    }
}
