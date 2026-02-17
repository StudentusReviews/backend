using AnonymousStudentReviews.Core.Aggregates.Review;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class ReviewOutboxConfiguration : IEntityTypeConfiguration<ReviewOutbox>
{
    public void Configure(EntityTypeBuilder<ReviewOutbox> builder)
    {
        builder
            .ToTable("review_outbox");

        builder
            .HasKey(e => e.Id);

        builder
            .HasOne(e => e.State)
            .WithMany(e => e.ReviewOutboxItems)
            .HasForeignKey(e => e.StateId)
            .IsRequired();

        builder
            .HasOne(e => e.Action)
            .WithMany(e => e.ReviewOutboxes)
            .HasForeignKey(e => e.ActionId)
            .IsRequired();

        builder
            .OwnsOne(c => c.Payload, d
                =>
            {
                d.ToJson();
            });
    }
}
