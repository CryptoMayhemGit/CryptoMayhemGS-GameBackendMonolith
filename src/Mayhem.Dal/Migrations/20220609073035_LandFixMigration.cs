using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class LandFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Land_GameUser_GameUserId",
                schema: "nft",
                table: "Land");

            migrationBuilder.DropIndex(
                name: "IX_Land_GameUserId",
                schema: "nft",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "GameUserId",
                schema: "nft",
                table: "Land");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameUserId",
                schema: "nft",
                table: "Land",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Land_GameUserId",
                schema: "nft",
                table: "Land",
                column: "GameUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Land_GameUser_GameUserId",
                schema: "nft",
                table: "Land",
                column: "GameUserId",
                principalSchema: "dbo",
                principalTable: "GameUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
