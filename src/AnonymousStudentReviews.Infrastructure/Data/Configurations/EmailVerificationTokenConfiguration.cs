using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class
    EmailVerificationTokenConfiguration : IEntityTypeConfiguration<
    Core.Aggregates.EmailVerificationToken.EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<Core.Aggregates.EmailVerificationToken.EmailVerificationToken> builder)
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
