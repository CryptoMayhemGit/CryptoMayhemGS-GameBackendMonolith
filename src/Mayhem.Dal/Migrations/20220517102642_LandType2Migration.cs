using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class LandType2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Mountains");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Field");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Farm");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Urban");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Ruins");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Default");

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[] { 0, null, null, "Forrest" });
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
                value: "Forrest");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Mountains");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Field");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Farm");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Urban");

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Ruins");

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[] { 7, null, null, "Default" });
        }
    }
}
