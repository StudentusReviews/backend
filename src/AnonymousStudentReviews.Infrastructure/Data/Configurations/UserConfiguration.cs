using AnonymousStudentReviews.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Id);

        builder
            .HasOne(e => e.University)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.UniversityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(e => e.Roles)
            .WithMany(e => e.Users)
            .UsingEntity(
                r => r.HasOne(typeof(Role)).WithMany().OnDelete(DeleteBehavior.Restrict),
                l => l.HasOne(typeof(User)).WithMany().OnDelete(DeleteBehavior.Restrict));

        builder
            .Property(e => e.EmailConfirmed)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(e => e.EmailHash)
            .HasMaxLength(70);
    }
}
