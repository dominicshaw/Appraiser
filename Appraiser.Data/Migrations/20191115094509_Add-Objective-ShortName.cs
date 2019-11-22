using Microsoft.EntityFrameworkCore.Migrations;

namespace Appraiser.Data.Migrations
{
    public partial class AddObjectiveShortName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Objectives",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Objectives");
        }
    }
}
