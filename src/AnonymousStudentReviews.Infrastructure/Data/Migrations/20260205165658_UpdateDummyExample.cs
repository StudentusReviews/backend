using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDummyExample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "dummies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_dummies_UserId",
                table: "dummies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_dummies_users_UserId",
                table: "dummies",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dummies_users_UserId",
                table: "dummies");

            migrationBuilder.DropIndex(
                name: "IX_dummies_UserId",
                table: "dummies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "dummies");
        }
    }
}
