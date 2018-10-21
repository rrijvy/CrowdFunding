using Microsoft.EntityFrameworkCore.Migrations;

namespace CrowdFunding.Data.Migrations
{
    public partial class AddImageAttributeToProjectClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Image3",
                table: "Projects");
        }
    }
}
