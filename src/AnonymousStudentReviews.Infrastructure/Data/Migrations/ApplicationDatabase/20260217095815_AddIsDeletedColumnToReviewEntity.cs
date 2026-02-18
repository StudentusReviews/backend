using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations.ApplicationDatabase
{
    /// <inheritdoc />
    public partial class AddIsDeletedColumnToReviewEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "reviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "reviews");
        }
    }
}
