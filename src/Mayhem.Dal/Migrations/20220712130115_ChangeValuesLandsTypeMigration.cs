using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class ChangeValuesLandsTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Forest");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Mountain");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Desert");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Field");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Biome1");

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[] { 0, null, null, "Swamp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Swamp");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Forest");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Mountain");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Desert");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Field");

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[] { 6, null, null, "Biome1" });
        }
    }
}
