using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Appraiser.Data.Migrations
{
    public partial class ChangesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Changes",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    When = table.Column<DateTime>(),
                    Username = table.Column<string>(nullable: true),
                    Table = table.Column<string>(nullable: true),
                    Field = table.Column<string>(nullable: true),
                    KeyId = table.Column<int>(),
                    Old = table.Column<string>(nullable: true),
                    New = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Changes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Changes");
        }
    }
}
