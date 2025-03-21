using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class AddIndexToMissionTablesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExploreMission_NpcId_LandId",
                schema: "mission",
                table: "ExploreMission",
                columns: new[] { "NpcId", "LandId" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryMission_NpcId_LandId",
                schema: "mission",
                table: "DiscoveryMission",
                columns: new[] { "NpcId", "LandId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExploreMission_NpcId_LandId",
                schema: "mission",
                table: "ExploreMission");

            migrationBuilder.DropIndex(
                name: "IX_DiscoveryMission_NpcId_LandId",
                schema: "mission",
                table: "DiscoveryMission");
        }
    }
}
