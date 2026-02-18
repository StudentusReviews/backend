using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeRoleNameAUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_roles_Name",
                table: "roles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_roles_Name",
                table: "roles");
        }
    }
}
