using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class NotificationAttempts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                schema: "dbo",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Land_LandInstanceId",
                schema: "nft",
                table: "Land",
                column: "LandInstanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Land_LandInstanceId",
                schema: "nft",
                table: "Land");

            migrationBuilder.DropColumn(
                name: "Attempts",
                schema: "dbo",
                table: "Notification");
        }
    }
}
