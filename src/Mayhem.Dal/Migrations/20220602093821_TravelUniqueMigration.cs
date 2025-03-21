using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class TravelUniqueMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Travel_NpcId_LandFromId_LandToId",
                schema: "dbo",
                table: "Travel");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_NpcId_LandFromId_LandToId",
                schema: "dbo",
                table: "Travel",
                columns: new[] { "NpcId", "LandFromId", "LandToId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Travel_NpcId_LandFromId_LandToId",
                schema: "dbo",
                table: "Travel");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_NpcId_LandFromId_LandToId",
                schema: "dbo",
                table: "Travel",
                columns: new[] { "NpcId", "LandFromId", "LandToId" });
        }
    }
}
