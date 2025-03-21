using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class OnSaleFlagUserLandMigraton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnSale",
                schema: "dbo",
                table: "UserLand",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnSale",
                schema: "dbo",
                table: "UserLand");
        }
    }
}
