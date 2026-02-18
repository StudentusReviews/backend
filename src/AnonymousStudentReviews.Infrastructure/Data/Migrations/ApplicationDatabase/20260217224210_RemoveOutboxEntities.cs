using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnonymousStudentReviews.Infrastructure.Data.Migrations.ApplicationDatabase
{
    /// <inheritdoc />
    public partial class RemoveOutboxEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "review_outbox");

            migrationBuilder.DropTable(
                name: "university_statistics");

            migrationBuilder.DropTable(
                name: "review_outbox_actions");

            migrationBuilder.DropTable(
                name: "review_outbox_states");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "review_outbox_states",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_outbox_states", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "university_statistics",
                columns: table => new
                {
                    UniversityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalReviewCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TotalScoreSum = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
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

            migrationBuilder.CreateTable(
                name: "review_outbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionId = table.Column<Guid>(type: "uuid", nullable: false),
                    StateId = table.Column<Guid>(type: "uuid", nullable: false),
                    UniversityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Payload = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_outbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_outbox_review_outbox_actions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "review_outbox_actions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_outbox_review_outbox_states_StateId",
                        column: x => x.StateId,
                        principalTable: "review_outbox_states",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_outbox_universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_ActionId",
                table: "review_outbox",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_StateId",
                table: "review_outbox",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_UniversityId",
                table: "review_outbox",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_actions_Name",
                table: "review_outbox_actions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_outbox_states_Name",
                table: "review_outbox_states",
                column: "Name",
                unique: true);
        }
    }
}
