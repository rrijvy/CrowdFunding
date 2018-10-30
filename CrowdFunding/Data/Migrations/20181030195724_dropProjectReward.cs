using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrowdFunding.Data.Migrations
{
    public partial class dropProjectReward : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRewards");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "investmentTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_investmentTypes_ProjectId",
                table: "investmentTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_investmentTypes_Projects_ProjectId",
                table: "investmentTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_investmentTypes_Projects_ProjectId",
                table: "investmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_investmentTypes_ProjectId",
                table: "investmentTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "investmentTypes");

            migrationBuilder.CreateTable(
                name: "ProjectRewards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    RewardDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRewards_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRewards_ProjectId",
                table: "ProjectRewards",
                column: "ProjectId");
        }
    }
}
