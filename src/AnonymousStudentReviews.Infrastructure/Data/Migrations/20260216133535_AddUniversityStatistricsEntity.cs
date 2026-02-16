using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversityStatistricsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "university_statistics",
                columns: table => new
                {
                    UniversityId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalScoreSum = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalReviewCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Rank = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_university_statistics", x => x.UniversityId);
                    table.ForeignKey(
                        name: "FK_university_statistics_universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "university_statistics");
        }
    }
}
