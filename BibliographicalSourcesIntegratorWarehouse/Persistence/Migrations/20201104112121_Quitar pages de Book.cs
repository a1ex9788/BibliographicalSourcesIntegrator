using Microsoft.EntityFrameworkCore.Migrations;

namespace BibliographicalSourcesIntegratorWarehouse.Migrations
{
    public partial class QuitarpagesdeBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pages",
                table: "Publications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pages",
                table: "Publications",
                type: "int",
                nullable: true);
        }
    }
}
