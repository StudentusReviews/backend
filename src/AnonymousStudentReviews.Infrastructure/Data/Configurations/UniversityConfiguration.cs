using AnonymousStudentReviews.Core;
using AnonymousStudentReviews.Core.DummyAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class UniversityConfiguration
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
    }
}
