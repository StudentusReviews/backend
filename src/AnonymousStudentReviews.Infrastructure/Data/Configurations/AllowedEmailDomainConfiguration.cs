using AnonymousStudentReviews.Core;
using AnonymousStudentReviews.Core.AllowedEmailDomainAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class AllowedEmailDomainConfiguration : IEntityTypeConfiguration<AllowedEmailDomain>
{
    public void Configure(EntityTypeBuilder<AllowedEmailDomain> builder)
    {
        builder.ToTable("allowed_email_domains");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Domain)
            .HasMaxLength(255)
            .IsRequired();
    }
}
