#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations.ApplicationDatabase;

/// <inheritdoc />
public partial class AddReviews : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "reviews",
            table => new
            {
                Id = table.Column<Guid>("uuid", nullable: false),
                UniversityId = table.Column<Guid>("uuid", nullable: false),
                UserId = table.Column<Guid>("uuid", nullable: false),
                Score = table.Column<int>("integer", nullable: false),
                Body = table.Column<string>("character varying(4000)", maxLength: 4000, nullable: false),
                CreatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>("timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_reviews", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            "IX_reviews_UniversityId_UserId",
            "reviews",
            new[] { "UniversityId", "UserId" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "reviews");
    }
}
