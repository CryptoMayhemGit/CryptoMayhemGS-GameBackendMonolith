using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class UserLandFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Land_GameUser_UserId",
                schema: "nft",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "IsBase",
                schema: "nft",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                schema: "nft",
                table: "Land");

            migrationBuilder.RenameColumn(
                name: "LandStatusId",
                schema: "dbo",
                table: "UserLand",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "nft",
                table: "Land",
                newName: "GameUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Land_UserId",
                schema: "nft",
                table: "Land",
                newName: "IX_Land_GameUserId");

            migrationBuilder.AddColumn<bool>(
                name: "HasFog",
                schema: "dbo",
                table: "UserLand",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Owned",
                schema: "dbo",
                table: "UserLand",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Land_GameUser_GameUserId",
                schema: "nft",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "HasFog",
                schema: "dbo",
                table: "UserLand");

            migrationBuilder.DropColumn(
                name: "Owned",
                schema: "dbo",
                table: "UserLand");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "dbo",
                table: "UserLand",
                newName: "LandStatusId");

            migrationBuilder.RenameColumn(
                name: "GameUserId",
                schema: "nft",
                table: "Land",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Land_GameUserId",
                schema: "nft",
                table: "Land",
                newName: "IX_Land_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsBase",
                schema: "nft",
                table: "Land",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                schema: "nft",
                table: "Land",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Land_GameUser_UserId",
                schema: "nft",
                table: "Land",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "GameUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
