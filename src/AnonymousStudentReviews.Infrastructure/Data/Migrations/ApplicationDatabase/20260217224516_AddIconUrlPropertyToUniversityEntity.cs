using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations.ApplicationDatabase
{
    /// <inheritdoc />
    public partial class AddIconUrlPropertyToUniversityEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconUrl",
                table: "universities",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconUrl",
                table: "universities");
        }
    }
}
