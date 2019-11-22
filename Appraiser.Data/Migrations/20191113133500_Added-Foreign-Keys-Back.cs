using Microsoft.EntityFrameworkCore.Migrations;

namespace Appraiser.Data.Migrations
{
    public partial class AddedForeignKeysBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
