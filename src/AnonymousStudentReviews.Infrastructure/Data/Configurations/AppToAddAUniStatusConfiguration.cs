using AnonymousStudentReviews.Core.Aggregates.AppToAddAUni;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class AppToAddAUniStatusConfiguration : IEntityTypeConfiguration<AppToAddAUniStatus>
{
    public void Configure(EntityTypeBuilder<AppToAddAUniStatus> builder)
    {
        builder.ToTable("application_statuses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();
    }
}
