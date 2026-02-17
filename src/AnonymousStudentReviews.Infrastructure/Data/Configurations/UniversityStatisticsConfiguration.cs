using AnonymousStudentReviews.Core.Aggregates.University;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class UniversityStatisticsConfiguration : IEntityTypeConfiguration<UniversityStatistics>
{
    public void Configure(EntityTypeBuilder<UniversityStatistics> builder)
    {
        builder
            .ToTable("university_statistics");

        builder
            .HasKey(e => e.UniversityId);

        builder
            .HasOne(e => e.University)
            .WithOne(e => e.UniversityStatistics)
            .HasForeignKey<UniversityStatistics>(e => e.UniversityId)
            .IsRequired();

        builder
            .Property(e => e.TotalReviewCount)
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(e => e.TotalScoreSum)
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(e => e.Rank)
            .HasDefaultValue(0)
            .IsRequired();
    }
}
