using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectsSoftuni.Data.Migrations
{
    public partial class CreateBudgetNullableInProjectEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Budget",
                table: "Projects",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Budget",
                table: "Projects",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
