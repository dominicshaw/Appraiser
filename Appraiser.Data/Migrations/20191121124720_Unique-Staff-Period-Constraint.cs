using Microsoft.EntityFrameworkCore.Migrations;

namespace Appraiser.Data.Migrations
{
    public partial class UniqueStaffPeriodConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appraisals_StaffId",
                table: "Appraisals");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_StaffId_PeriodStart_Deleted",
                table: "Appraisals",
                columns: new[] { "StaffId", "PeriodStart", "Deleted" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appraisals_StaffId_PeriodStart_Deleted",
                table: "Appraisals");

            migrationBuilder.CreateIndex(
                name: "IX_Appraisals_StaffId",
                table: "Appraisals",
                column: "StaffId");
        }
    }
}
