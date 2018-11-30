using Microsoft.EntityFrameworkCore.Migrations;

namespace CrowdFunding.Data.Migrations
{
    public partial class forFavouriteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_EntrepreneurId",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "EntrepreneurId",
                table: "Companies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_EntrepreneurId",
                table: "Companies",
                column: "EntrepreneurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AspNetUsers_EntrepreneurId",
                table: "Companies");

            migrationBuilder.AlterColumn<string>(
                name: "EntrepreneurId",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AspNetUsers_EntrepreneurId",
                table: "Companies",
                column: "EntrepreneurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
