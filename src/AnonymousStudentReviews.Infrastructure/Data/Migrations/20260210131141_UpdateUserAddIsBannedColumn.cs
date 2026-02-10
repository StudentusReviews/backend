using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAddIsBannedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "users");
        }
    }
}
