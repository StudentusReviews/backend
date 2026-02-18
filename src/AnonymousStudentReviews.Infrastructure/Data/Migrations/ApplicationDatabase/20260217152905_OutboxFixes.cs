using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations.ApplicationDatabase
{
    /// <inheritdoc />
    public partial class OutboxFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_review_outbox_reviews_ReviewId",
                table: "review_outbox");

            migrationBuilder.DropIndex(
                name: "IX_review_outbox_ReviewId",
                table: "review_outbox");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "review_outbox",
                newName: "ActionId");

            migrationBuilder.AddColumn<string>(
                name: "Payload",
                table: "review_outbox",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.CreateTable(
                name: "review_outbox_actions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_outbox_actions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_ActionId",
                table: "review_outbox",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_actions_Name",
                table: "review_outbox_actions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_review_outbox_review_outbox_actions_ActionId",
                table: "review_outbox",
                column: "ActionId",
                principalTable: "review_outbox_actions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_review_outbox_review_outbox_actions_ActionId",
                table: "review_outbox");

            migrationBuilder.DropTable(
                name: "review_outbox_actions");

            migrationBuilder.DropIndex(
                name: "IX_review_outbox_ActionId",
                table: "review_outbox");

            migrationBuilder.DropColumn(
                name: "Payload",
                table: "review_outbox");

            migrationBuilder.RenameColumn(
                name: "ActionId",
                table: "review_outbox",
                newName: "ReviewId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "reviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_ReviewId",
                table: "review_outbox",
                column: "ReviewId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_review_outbox_reviews_ReviewId",
                table: "review_outbox",
                column: "ReviewId",
                principalTable: "reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
