using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class MissionsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscoveryMission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NpcId = table.Column<long>(type: "bigint", nullable: false),
                    LandId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveryMission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscoveryMission_GameUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "GameUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscoveryMission_Land_LandId",
                        column: x => x.LandId,
                        principalSchema: "nft",
                        principalTable: "Land",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscoveryMission_Npc_NpcId",
                        column: x => x.NpcId,
                        principalSchema: "nft",
                        principalTable: "Npc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExploreMission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NpcId = table.Column<long>(type: "bigint", nullable: false),
                    LandId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExploreMission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExploreMission_GameUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "GameUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExploreMission_Land_LandId",
                        column: x => x.LandId,
                        principalSchema: "nft",
                        principalTable: "Land",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExploreMission_Npc_NpcId",
                        column: x => x.NpcId,
                        principalSchema: "nft",
                        principalTable: "Npc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryMission_LandId",
                schema: "dbo",
                table: "DiscoveryMission",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryMission_NpcId",
                schema: "dbo",
                table: "DiscoveryMission",
                column: "NpcId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveryMission_UserId",
                schema: "dbo",
                table: "DiscoveryMission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExploreMission_LandId",
                schema: "dbo",
                table: "ExploreMission",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_ExploreMission_NpcId",
                schema: "dbo",
                table: "ExploreMission",
                column: "NpcId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExploreMission_UserId",
                schema: "dbo",
                table: "ExploreMission",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscoveryMission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ExploreMission",
                schema: "dbo");
        }
    }
}
