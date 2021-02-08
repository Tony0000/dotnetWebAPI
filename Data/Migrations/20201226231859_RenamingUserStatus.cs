using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RenamingUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
