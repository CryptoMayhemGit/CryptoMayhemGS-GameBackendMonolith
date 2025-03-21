using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class AddStatusToNpcMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NpcStatusId",
                schema: "nft",
                table: "Npc",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NpcStatus",
                schema: "dictionary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NpcStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "NpcStatus",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "None" },
                    { 2, null, null, "OnTravel" },
                    { 3, null, null, "OnExploreMission" },
                    { 4, null, null, "OnDiscoveryMission" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Npc_NpcStatusId",
                schema: "nft",
                table: "Npc",
                column: "NpcStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Npc_NpcStatus_NpcStatusId",
                schema: "nft",
                table: "Npc",
                column: "NpcStatusId",
                principalSchema: "dictionary",
                principalTable: "NpcStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Npc_NpcStatus_NpcStatusId",
                schema: "nft",
                table: "Npc");

            migrationBuilder.DropTable(
                name: "NpcStatus",
                schema: "dictionary");

            migrationBuilder.DropIndex(
                name: "IX_Npc_NpcStatusId",
                schema: "nft",
                table: "Npc");

            migrationBuilder.DropColumn(
                name: "NpcStatusId",
                schema: "nft",
                table: "Npc");
        }
    }
}
