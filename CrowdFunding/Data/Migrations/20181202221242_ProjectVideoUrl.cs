using Microsoft.EntityFrameworkCore.Migrations;

namespace CrowdFunding.Data.Migrations
{
    public partial class ProjectVideoUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Projects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Projects");
        }
    }
}
