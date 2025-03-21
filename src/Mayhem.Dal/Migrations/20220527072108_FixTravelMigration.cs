using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class FixTravelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Travel_NpcId",
                schema: "dbo",
                table: "Travel");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                schema: "dbo",
                table: "Travel",
                newName: "FinishDate");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_NpcId_LandFromId_LandToId",
                schema: "dbo",
                table: "Travel",
                columns: new[] { "NpcId", "LandFromId", "LandToId" });

            migrationBuilder.CreateIndex(
                name: "IX_Land_PositionX_PositionY",
                schema: "nft",
                table: "Land",
                columns: new[] { "PositionX", "PositionY" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Travel_NpcId_LandFromId_LandToId",
                schema: "dbo",
                table: "Travel");

            migrationBuilder.DropIndex(
                name: "IX_Land_PositionX_PositionY",
                schema: "nft",
                table: "Land");

            migrationBuilder.RenameColumn(
                name: "FinishDate",
                schema: "dbo",
                table: "Travel",
                newName: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_NpcId",
                schema: "dbo",
                table: "Travel",
                column: "NpcId",
                unique: true);
        }
    }
}
