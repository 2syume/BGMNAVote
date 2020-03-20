using Microsoft.EntityFrameworkCore.Migrations;

namespace BGMNANotebookGrab.Migrations
{
    public partial class AddAlignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alignment",
                table: "Vote",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alignment",
                table: "Vote");
        }
    }
}
