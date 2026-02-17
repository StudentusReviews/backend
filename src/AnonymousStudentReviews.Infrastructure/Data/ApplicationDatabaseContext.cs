using AnonymousStudentReviews.Core.Aggregates.AllowedEmailDomain;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.Core.Aggregates.Review;
using AnonymousStudentReviews.Core.Aggregates.Role;
using AnonymousStudentReviews.Core.Aggregates.University;
using AnonymousStudentReviews.Core.Aggregates.User;
using AnonymousStudentReviews.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;

namespace AnonymousStudentReviews.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    protected ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Dummy> Dummies { get; set; }
    public DbSet<University> Universities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<AllowedEmailDomain> AllowedEmailDomains { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Core.Aggregates.EmailVerificationToken.EmailVerificationToken> EmailVerificationTokens { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<UniversityStatistics> UniversityStatistics { get; set; }
    public DbSet<ReviewOutbox> ReviewOutbox { get; set; }
    public DbSet<ReviewOutboxStateEntity> ReviewOutboxStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DummyConfiguration).Assembly);
    }
}
