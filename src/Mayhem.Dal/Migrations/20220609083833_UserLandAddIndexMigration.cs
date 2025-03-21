using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class UserLandAddIndexMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserLand_LandId_UserId",
                schema: "dbo",
                table: "UserLand",
                columns: new[] { "LandId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserLand_LandId_UserId",
                schema: "dbo",
                table: "UserLand");
        }
    }
}
