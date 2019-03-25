using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectsSoftuni.Data.Migrations
{
    public partial class EditPropertyFinishDateInProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinishDate",
                table: "Projects",
                newName: "DueDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Projects",
                newName: "FinishDate");
        }
    }
}
