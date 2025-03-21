using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class LandTypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 0);

            migrationBuilder.DeleteData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Default");

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[] { -1, null, null, "Water" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.UpdateData(
                schema: "dictionary",
                table: "LandType",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Water");

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[] { 8, null, null, "Default" });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[] { 0, null, null, "None" });
        }
    }
}
