using AnonymousStudentReviews.Core.Aggregates;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.ToTable("email_verification_codes");

        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.TokenHash)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .HasOne(e => e.User);
    }
}
