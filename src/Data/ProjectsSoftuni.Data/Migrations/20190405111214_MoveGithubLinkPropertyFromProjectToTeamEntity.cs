using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectsSoftuni.Data.Migrations
{
    public partial class MoveGithubLinkPropertyFromProjectToTeamEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubLink",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "GitHubLink",
                table: "Teams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHubLink",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "GitHubLink",
                table: "Projects",
                nullable: true);
        }
    }
}
