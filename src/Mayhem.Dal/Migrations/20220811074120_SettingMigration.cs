using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class SettingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "setting");

            migrationBuilder.CreateTable(
                name: "Setting",
                schema: "setting",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Key);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setting",
                schema: "setting");
        }
    }
}
