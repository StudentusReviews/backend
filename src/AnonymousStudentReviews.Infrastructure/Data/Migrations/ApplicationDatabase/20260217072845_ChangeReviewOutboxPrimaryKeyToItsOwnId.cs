using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReviewOutboxPrimaryKeyToItsOwnId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_review_outbox",
                table: "review_outbox");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "review_outbox",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_review_outbox",
                table: "review_outbox",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_ReviewId",
                table: "review_outbox",
                column: "ReviewId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_review_outbox",
                table: "review_outbox");

            migrationBuilder.DropIndex(
                name: "IX_review_outbox_ReviewId",
                table: "review_outbox");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "review_outbox");

            migrationBuilder.AddPrimaryKey(
                name: "PK_review_outbox",
                table: "review_outbox",
                column: "ReviewId");
        }
    }
}
