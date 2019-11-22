using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Appraiser.Data.Migrations
{
    public partial class AddResponsibilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Responsibilities",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(),
                    RoleTitle = table.Column<string>(maxLength: 100, nullable: true),
                    KeySkills = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmployeeSignoff = table.Column<DateTime>(nullable: true),
                    ManagerSignoff = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsibilities_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responsibilities_StaffId",
                table: "Responsibilities",
                column: "StaffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Responsibilities");
        }
    }
}
