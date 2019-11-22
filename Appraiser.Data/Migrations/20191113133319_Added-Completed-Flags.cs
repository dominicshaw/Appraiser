using Microsoft.EntityFrameworkCore.Migrations;

namespace Appraiser.Data.Migrations
{
    public partial class AddedCompletedFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appraisals_Reviews_FullYearReviewId",
                table: "Appraisals");

            migrationBuilder.DropForeignKey(
                name: "FK_Appraisals_Reviews_MidYearReviewId",
                table: "Appraisals");

            migrationBuilder.DropIndex(
                name: "IX_Appraisals_FullYearReviewId",
                table: "Appraisals");

            migrationBuilder.DropIndex(
                name: "IX_Appraisals_MidYearReviewId",
                table: "Appraisals");

            migrationBuilder.AddColumn<bool>(
                name: "Complete",
                table: "Reviews",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Complete",
                table: "Appraisals",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complete",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Complete",
                table: "Appraisals");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_FullYearReviewId",
                table: "Appraisals",
                column: "FullYearReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_MidYearReviewId",
                table: "Appraisals",
                column: "MidYearReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appraisals_Reviews_FullYearReviewId",
                table: "Appraisals",
                column: "FullYearReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appraisals_Reviews_MidYearReviewId",
                table: "Appraisals",
                column: "MidYearReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
