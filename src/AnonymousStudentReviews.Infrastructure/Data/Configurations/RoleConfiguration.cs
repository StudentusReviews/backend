using AnonymousStudentReviews.Core.DummyAggregate;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnonymousStudentReviews.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Dummy>
{
    public void Configure(EntityTypeBuilder<Dummy> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
