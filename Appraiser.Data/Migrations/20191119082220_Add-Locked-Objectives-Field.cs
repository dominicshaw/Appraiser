using Microsoft.EntityFrameworkCore.Migrations;

namespace Appraiser.Data.Migrations
{
    public partial class AddLockedObjectivesField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ObjectivesLocked",
                table: "Appraisals",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectivesLocked",
                table: "Appraisals");
        }
    }
}
