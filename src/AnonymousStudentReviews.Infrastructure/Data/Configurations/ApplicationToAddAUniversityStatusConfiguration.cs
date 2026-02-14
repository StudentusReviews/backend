using AnonymousStudentReviews.Core.Aggregates.ApplicationToAddAUniversity.Status;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class ApplicationToAddAUniversityStatusConfiguration : IEntityTypeConfiguration<ApplicationToAddAUniversityStatus>
{
    public void Configure(EntityTypeBuilder<ApplicationToAddAUniversityStatus> builder)
    {
        builder.ToTable("application_statuses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
