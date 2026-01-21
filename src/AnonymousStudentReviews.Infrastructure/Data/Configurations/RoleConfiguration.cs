using AnonymousStudentReviews.Core;
using AnonymousStudentReviews.Core.DummyAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
