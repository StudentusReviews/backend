using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class ApplicationToAddAUniversityConfiguration : IEntityTypeConfiguration<ApplicationToAddAUniversity>
{
    public void Configure(EntityTypeBuilder<ApplicationToAddAUniversity> builder)
    {
        builder.ToTable("applications_to_add_a_university");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.UniversityName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.DomainName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Comment)
            .HasMaxLength(500);

        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .HasOne(e => e.User)
            .WithMany(e => e.ApplicationToAddAUniversities)
            .HasForeignKey(e => e.UserId)
            .IsRequired();

        builder
            .HasOne(e => e.ApplicationToAddAUniversityStatus)
            .WithMany(e => e.ApplicationToAddAUniversities)
            .HasForeignKey(e => e.ApplicationToAddAUniversityStatusId)
            .IsRequired();

    }
}
