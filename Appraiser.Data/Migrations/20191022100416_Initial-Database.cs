using System;
using Microsoft.EntityFrameworkCore.Migrations;

// ReSharper disable All

namespace Appraiser.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerNotes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logon = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ManagerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Staff_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TrainingType = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appraisals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(nullable: false),
                    PeriodStart = table.Column<DateTime>(nullable: false),
                    PeriodEnd = table.Column<DateTime>(nullable: false),
                    MidYearReviewId = table.Column<int>(nullable: true),
                    FullYearReviewId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appraisals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appraisals_Reviews_FullYearReviewId",
                        column: x => x.FullYearReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appraisals_Reviews_MidYearReviewId",
                        column: x => x.MidYearReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appraisals_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Objectives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppraisalId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Measurement = table.Column<string>(nullable: true),
                    Weight = table.Column<decimal>(nullable: false),
                    Achieved = table.Column<decimal>(nullable: true),
                    ManagerNotes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objectives_Appraisals_AppraisalId",
                        column: x => x.AppraisalId,
                        principalTable: "Appraisals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_FullYearReviewId",
                table: "Appraisals",
                column: "FullYearReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_MidYearReviewId",
                table: "Appraisals",
                column: "MidYearReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_StaffId",
                table: "Appraisals",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_AppraisalId",
                table: "Objectives",
                column: "AppraisalId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_ManagerId",
                table: "Staff",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_ReviewId",
                table: "Training",
                column: "ReviewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Objectives");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "Appraisals");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Staff");
        }
    }
}
