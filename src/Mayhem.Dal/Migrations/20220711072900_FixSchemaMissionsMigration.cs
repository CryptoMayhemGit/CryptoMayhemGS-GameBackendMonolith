using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class FixSchemaMissionsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mission");

            migrationBuilder.RenameTable(
                name: "ExploreMission",
                schema: "dbo",
                newName: "ExploreMission",
                newSchema: "mission");

            migrationBuilder.RenameTable(
                name: "DiscoveryMission",
                schema: "dbo",
                newName: "DiscoveryMission",
                newSchema: "mission");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ExploreMission",
                schema: "mission",
                newName: "ExploreMission",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "DiscoveryMission",
                schema: "mission",
                newName: "DiscoveryMission",
                newSchema: "dbo");
        }
    }
}
