using AnonymousStudentReviews.Core.Aggregates.Review;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class ReviewOutboxStateConfiguration : IEntityTypeConfiguration<ReviewOutboxStateEntity>
{
    public void Configure(EntityTypeBuilder<ReviewOutboxStateEntity> builder)
    {
        builder
            .ToTable("review_outbox_states");

        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .HasIndex(e => e.Name)
            .IsUnique();
    }
}
