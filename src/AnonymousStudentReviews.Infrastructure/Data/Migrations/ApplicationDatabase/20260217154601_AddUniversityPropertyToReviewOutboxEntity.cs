using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations.ApplicationDatabase
{
    /// <inheritdoc />
    public partial class AddUniversityPropertyToReviewOutboxEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UniversityId",
                table: "review_outbox",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_UniversityId",
                table: "review_outbox",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_review_outbox_universities_UniversityId",
                table: "review_outbox",
                column: "UniversityId",
                principalTable: "universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_review_outbox_universities_UniversityId",
                table: "review_outbox");

            migrationBuilder.DropIndex(
                name: "IX_review_outbox_UniversityId",
                table: "review_outbox");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "review_outbox");
        }
    }
}
