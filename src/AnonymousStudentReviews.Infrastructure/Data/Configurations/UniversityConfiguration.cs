using AnonymousStudentReviews.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class UniversityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder.ToTable("universities");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.City)
            .HasMaxLength(255);

        builder.Property(e => e.Website)
            .HasMaxLength(300);

        builder
            .HasMany(e => e.AllowedEmailDomains)
            .WithOne(e => e.University)
            .HasForeignKey(e => e.UniversityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
