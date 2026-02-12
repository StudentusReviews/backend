using AnonymousStudentReviews.Core;
using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;
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
    public DbSet<User> Users { get; set; }
    public DbSet<AllowedEmailDomain> AllowedEmailDomains { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Core.Aggregates.EmailVerificationToken.EmailVerificationToken> EmailVerificationTokens { get; set; }
    public DbSet<AppToAddAUni> Applications { get; set; }
    public DbSet<AppToAddAUniStatus> ApplicationStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DummyConfiguration).Assembly);
    }
}
