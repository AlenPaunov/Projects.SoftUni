using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectsSoftuni.Data.Migrations
{
    public partial class CreateSpecificationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecificationId",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Specifications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    SpecificationId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SpecificationId",
                table: "Projects",
                column: "SpecificationId",
                unique: true,
                filter: "[SpecificationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Specifications_SpecificationId",
                table: "Projects",
                column: "SpecificationId",
                principalTable: "Specifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Specifications_SpecificationId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Specifications");

            migrationBuilder.DropIndex(
                name: "IX_Projects_SpecificationId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SpecificationId",
                table: "Projects");
        }
    }
}
