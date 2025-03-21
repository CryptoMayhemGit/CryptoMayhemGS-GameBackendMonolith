using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Mayhem.Dal.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "dictionary");

            migrationBuilder.EnsureSchema(
                name: "logs");

            migrationBuilder.EnsureSchema(
                name: "building");

            migrationBuilder.EnsureSchema(
                name: "guild");

            migrationBuilder.EnsureSchema(
                name: "hc");

            migrationBuilder.EnsureSchema(
                name: "nft");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributeType",
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
                    table.PrimaryKey("PK_AttributeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Wallet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SignedMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Nonce = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockType",
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
                    table.PrimaryKey("PK_BlockType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingBonusType",
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
                    table.PrimaryKey("PK_BuildingBonusType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BuildingType",
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
                    table.PrimaryKey("PK_BuildingType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildBuildingBonusType",
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
                    table.PrimaryKey("PK_GuildBuildingBonusType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildBuildingType",
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
                    table.PrimaryKey("PK_GuildBuildingType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildImprovementType",
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
                    table.PrimaryKey("PK_GuildImprovementType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealthCheck",
                schema: "hc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthCheck", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImprovementType",
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
                    table.PrimaryKey("PK_ImprovementType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemBonusType",
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
                    table.PrimaryKey("PK_ItemBonusType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemType",
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
                    table.PrimaryKey("PK_ItemType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandInstance",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandInstance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandType",
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
                    table.PrimaryKey("PK_LandType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WasSent = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NpcType",
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
                    table.PrimaryKey("PK_NpcType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceType",
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
                    table.PrimaryKey("PK_ResourceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Block",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastBlock = table.Column<long>(type: "bigint", nullable: false),
                    BlockTypeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Block_BlockType_BlockTypeId",
                        column: x => x.BlockTypeId,
                        principalSchema: "dictionary",
                        principalTable: "BlockType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NpcId = table.Column<long>(type: "bigint", nullable: false),
                    AttributeTypeId = table.Column<int>(type: "int", nullable: false),
                    BaseValue = table.Column<double>(type: "float", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_AttributeType_AttributeTypeId",
                        column: x => x.AttributeTypeId,
                        principalSchema: "dictionary",
                        principalTable: "AttributeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BuildingBonus",
                schema: "building",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingId = table.Column<long>(type: "bigint", nullable: false),
                    BuildingBonusTypeId = table.Column<int>(type: "int", nullable: false),
                    Bonus = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuildingBonus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuildingBonus_BuildingBonusType_BuildingBonusTypeId",
                        column: x => x.BuildingBonusTypeId,
                        principalSchema: "dictionary",
                        principalTable: "BuildingBonusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Npc",
                schema: "nft",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingId = table.Column<long>(type: "bigint", nullable: true),
                    NpcTypeId = table.Column<int>(type: "int", nullable: false),
                    NpcHealthStateId = table.Column<int>(type: "int", nullable: false),
                    IsAvatar = table.Column<bool>(type: "bit", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: true),
                    IsMinted = table.Column<bool>(type: "bit", nullable: false),
                    LandId = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Npc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Npc_NpcType_NpcTypeId",
                        column: x => x.NpcTypeId,
                        principalSchema: "dictionary",
                        principalTable: "NpcType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Building",
                schema: "building",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandId = table.Column<long>(type: "bigint", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    BuildingTypeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Building_BuildingType_BuildingTypeId",
                        column: x => x.BuildingTypeId,
                        principalSchema: "dictionary",
                        principalTable: "BuildingType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Guild",
                schema: "guild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guild", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameUser",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GuildId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameUser_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildBuilding",
                schema: "guild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    GuildBuildingTypeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBuilding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildBuilding_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildBuilding_GuildBuildingType_GuildBuildingTypeId",
                        column: x => x.GuildBuildingTypeId,
                        principalSchema: "dictionary",
                        principalTable: "GuildBuildingType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildImprovement",
                schema: "guild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    GuildImprovementTypeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildImprovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildImprovement_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildImprovement_GuildImprovementType_GuildImprovementTypeId",
                        column: x => x.GuildImprovementTypeId,
                        principalSchema: "dictionary",
                        principalTable: "GuildImprovementType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildResource",
                schema: "guild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    ResourceTypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildResource_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildResource_ResourceType_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalSchema: "dictionary",
                        principalTable: "ResourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildInvitation",
                schema: "guild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GuildId = table.Column<int>(type: "int", nullable: false),
                    InvitationType = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildInvitation_GameUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "GameUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildInvitation_Guild_GuildId",
                        column: x => x.GuildId,
                        principalSchema: "guild",
                        principalTable: "Guild",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "nft",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMinted = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_GameUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "GameUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_ItemType_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalSchema: "dictionary",
                        principalTable: "ItemType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Land",
                schema: "nft",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    LandTypeId = table.Column<int>(type: "int", nullable: false),
                    LandInstanceId = table.Column<int>(type: "int", nullable: false),
                    PositionX = table.Column<int>(type: "int", nullable: false),
                    PositionY = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMinted = table.Column<bool>(type: "bit", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsBase = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Land", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Land_GameUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "GameUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Land_LandInstance_LandInstanceId",
                        column: x => x.LandInstanceId,
                        principalSchema: "dbo",
                        principalTable: "LandInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Land_LandType_LandTypeId",
                        column: x => x.LandTypeId,
                        principalSchema: "dictionary",
                        principalTable: "LandType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserResource",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResourceTypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserResource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserResource_GameUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "GameUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserResource_ResourceType_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalSchema: "dictionary",
                        principalTable: "ResourceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildBuildingBonus",
                schema: "guild",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuildBuildingId = table.Column<int>(type: "int", nullable: false),
                    GuildBuildingBonusTypeId = table.Column<int>(type: "int", nullable: false),
                    Bonus = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBuildingBonus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildBuildingBonus_GuildBuilding_GuildBuildingId",
                        column: x => x.GuildBuildingId,
                        principalSchema: "guild",
                        principalTable: "GuildBuilding",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GuildBuildingBonus_GuildBuildingBonusType_GuildBuildingBonusTypeId",
                        column: x => x.GuildBuildingBonusTypeId,
                        principalSchema: "dictionary",
                        principalTable: "GuildBuildingBonusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemBonus",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    ItemBonusTypeId = table.Column<int>(type: "int", nullable: false),
                    Bonus = table.Column<double>(type: "float", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemBonus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemBonus_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "nft",
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemBonus_ItemBonusType_ItemBonusTypeId",
                        column: x => x.ItemBonusTypeId,
                        principalSchema: "dictionary",
                        principalTable: "ItemBonusType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Improvement",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandId = table.Column<long>(type: "bigint", nullable: false),
                    ImprovementTypeId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Improvement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Improvement_ImprovementType_ImprovementTypeId",
                        column: x => x.ImprovementTypeId,
                        principalSchema: "dictionary",
                        principalTable: "ImprovementType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Improvement_Land_LandId",
                        column: x => x.LandId,
                        principalSchema: "nft",
                        principalTable: "Land",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandId = table.Column<long>(type: "bigint", nullable: false),
                    NpcId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Job_Land_LandId",
                        column: x => x.LandId,
                        principalSchema: "nft",
                        principalTable: "Land",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Job_Npc_NpcId",
                        column: x => x.NpcId,
                        principalSchema: "nft",
                        principalTable: "Npc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Travel",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NpcId = table.Column<long>(type: "bigint", nullable: false),
                    LandFromId = table.Column<long>(type: "bigint", nullable: false),
                    LandToId = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Travel_Land_LandFromId",
                        column: x => x.LandFromId,
                        principalSchema: "nft",
                        principalTable: "Land",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Travel_Land_LandToId",
                        column: x => x.LandToId,
                        principalSchema: "nft",
                        principalTable: "Land",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Travel_Npc_NpcId",
                        column: x => x.NpcId,
                        principalSchema: "nft",
                        principalTable: "Npc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLand",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LandStatusId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    LastModificationDate = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLand_GameUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "GameUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLand_Land_LandId",
                        column: x => x.LandId,
                        principalSchema: "nft",
                        principalTable: "Land",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "AttributeType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "LightWoodProduction" },
                    { 15, null, null, "Detection" },
                    { 14, null, null, "Construction" },
                    { 13, null, null, "Repair" },
                    { 12, null, null, "Discovery" },
                    { 11, null, null, "CerealConsumption" },
                    { 10, null, null, "MeatConsumption" },
                    { 9, null, null, "MoveSpeed" },
                    { 16, null, null, "MechProduction" },
                    { 7, null, null, "Attack" },
                    { 6, null, null, "CerealProduction" },
                    { 5, null, null, "MeatProduction" },
                    { 4, null, null, "TitaniumProduction" },
                    { 3, null, null, "IronOreProduction" },
                    { 2, null, null, "HeavyWoodProduction" },
                    { 8, null, null, "Healing" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "BlockType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 3, null, null, "Item" },
                    { 4, null, null, "Land" },
                    { 1, null, null, "Avatar" },
                    { 2, null, null, "Npc" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "BuildingBonusType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Wood" },
                    { 2, null, null, "Mining" },
                    { 3, null, null, "Construction" },
                    { 4, null, null, "MechaniumCollection" },
                    { 5, null, null, "Attack" },
                    { 6, null, null, "Cereal" },
                    { 7, null, null, "Meat" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "BuildingType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 6, null, null, "Farm" },
                    { 8, null, null, "Guardhouse" },
                    { 7, null, null, "Slaughterhouse" },
                    { 5, null, null, "CombatWorkshop" },
                    { 3, null, null, "MechanicalWorkshop" },
                    { 2, null, null, "OreMine" },
                    { 1, null, null, "Lumbermill" },
                    { 4, null, null, "DroneFactory" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "GuildBuildingBonusType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "All" },
                    { 2, null, null, "DiscoveryDetection" },
                    { 3, null, null, "MechConstruction" },
                    { 4, null, null, "MechAttack" },
                    { 5, null, null, "MoveSpeed" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "GuildBuildingType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 2, null, null, "ExplorationBoard" },
                    { 1, null, null, "AdriaCorporationHeadquarters" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "GuildBuildingType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 3, null, null, "MechBoard" },
                    { 4, null, null, "FightBoard" },
                    { 5, null, null, "TransportBoard" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "GuildImprovementType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 21, null, null, "StrongerInternalStructure" },
                    { 20, null, null, "WeaponReinforcement" },
                    { 17, null, null, "ProductionMatrix" },
                    { 18, null, null, "ImprovedAssemblyLine" },
                    { 22, null, null, "MechaniumI" },
                    { 19, null, null, "ImprovedEnergyProcessing" },
                    { 23, null, null, "MechaniumII" },
                    { 30, null, null, "LogisticSupport" },
                    { 25, null, null, "LargerWheels" },
                    { 26, null, null, "ImprovedTransmission" },
                    { 27, null, null, "PowerfulEngine" },
                    { 28, null, null, "AdditionalDrive" },
                    { 29, null, null, "ImprovedFuelMixture" },
                    { 24, null, null, "MechaniumIII" },
                    { 16, null, null, "WasteManagement" },
                    { 4, null, null, "SupportPackage" },
                    { 14, null, null, "SheetMetalPressingPlant" },
                    { 15, null, null, "AssemblyLine" },
                    { 1, null, null, "RegenerativeMeal" },
                    { 2, null, null, "Flashlight" },
                    { 5, null, null, "NeuralConditioning" },
                    { 6, null, null, "SIControlled" },
                    { 7, null, null, "TerrainScanning" },
                    { 3, null, null, "Motivator" },
                    { 9, null, null, "SoilSampling" },
                    { 10, null, null, "MolecularAnalysis" },
                    { 11, null, null, "StatisticalAnalysis" },
                    { 12, null, null, "SatelliteReconnaissance" },
                    { 13, null, null, "PatternLibrary" },
                    { 8, null, null, "AerialReconnaissance" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "ImprovementType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 27, null, null, "HighEnergyPowder" },
                    { 35, null, null, "ProtectiveMeasures" },
                    { 28, null, null, "BattlefieldRadar" },
                    { 29, null, null, "LaserGuidance" },
                    { 30, null, null, "AssistedAI" },
                    { 31, null, null, "Seedling" },
                    { 32, null, null, "SelectedSoil" },
                    { 33, null, null, "Fertilizers" },
                    { 34, null, null, "PlantGrafting" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "ImprovementType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 37, null, null, "HealthyFeed" },
                    { 44, null, null, "ObservationDrone" },
                    { 39, null, null, "VeterinaryCare" },
                    { 40, null, null, "SustainableBreeding" },
                    { 41, null, null, "IncreasedAnimalWelfare" },
                    { 42, null, null, "GeneticSupport" },
                    { 43, null, null, "SituationalRecognition" },
                    { 45, null, null, "InfraredObservation" },
                    { 46, null, null, "FireControlDrone" },
                    { 47, null, null, "ReinforcedAmmunition" },
                    { 48, null, null, "ImprovedFireControlComputer" },
                    { 26, null, null, "WeaponModificationDrives" },
                    { 38, null, null, "NaturalSelectionControl" },
                    { 25, null, null, "FullShellMissiles" },
                    { 36, null, null, "GeneticModification" },
                    { 23, null, null, "PrecisePositioning" },
                    { 1, null, null, "ReinforcedChainsawMotor" },
                    { 2, null, null, "ImprovedGear" },
                    { 3, null, null, "HardenedSawChain" },
                    { 4, null, null, "TreeScanner" },
                    { 5, null, null, "EnergyCells" },
                    { 6, null, null, "LaserBlade" },
                    { 7, null, null, "TitaniumPickaxe" },
                    { 8, null, null, "DiamondHeadDrillBit" },
                    { 9, null, null, "BasicMiningCombine" },
                    { 11, null, null, "MiningShaftsVentilation" },
                    { 12, null, null, "OreElectrolyticRefining" },
                    { 10, null, null, "DeepExcavationsTechnology" },
                    { 13, null, null, "QuickDryingCement" },
                    { 14, null, null, "ReinforcedFoundations" },
                    { 15, null, null, "ModularMaterials" },
                    { 16, null, null, "SpecialistBrigade" },
                    { 17, null, null, "PrefabricatedElements" },
                    { 18, null, null, "ConstructionRobot" },
                    { 19, null, null, "ReinforcedRotors" },
                    { 20, null, null, "TitaniumHull" },
                    { 21, null, null, "EnlargedScraper" },
                    { 22, null, null, "AdditionalCovers" },
                    { 24, null, null, "AdditionalTankForMechanium" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "ItemBonusType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 9, null, null, "Repair" },
                    { 12, null, null, "MechProduction" },
                    { 11, null, null, "Detection" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "ItemBonusType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 10, null, null, "Construction" },
                    { 7, null, null, "MoveSpeed" },
                    { 8, null, null, "Discovery" },
                    { 5, null, null, "Attack" },
                    { 6, null, null, "Healing" },
                    { 4, null, null, "Cereal" },
                    { 3, null, null, "Meat" },
                    { 2, null, null, "Mining" },
                    { 1, null, null, "Wood" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "ItemType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 40, null, null, "MaterialContainer" },
                    { 39, null, null, "DiagnosticStation" },
                    { 38, null, null, "Distiller" },
                    { 37, null, null, "SetOfMeasuringCup" },
                    { 36, null, null, "TestContainer" },
                    { 34, null, null, "LaserProbe" },
                    { 41, null, null, "Truck" },
                    { 33, null, null, "HandProbe" },
                    { 32, null, null, "CraneOnTheCar" },
                    { 56, null, null, "HumanoidRobot" },
                    { 31, null, null, "MultiFunctionRobot" },
                    { 35, null, null, "InspectionCrate" },
                    { 42, null, null, "Van" },
                    { 45, null, null, "FirstAidKit" },
                    { 44, null, null, "ContainerTrailer" },
                    { 46, null, null, "SurgicalKit" },
                    { 47, null, null, "MedicalRobot" },
                    { 48, null, null, "MedicalContainer" },
                    { 49, null, null, "ScaleMeter" },
                    { 50, null, null, "SearchProbe" },
                    { 51, null, null, "DroneWithGeologicalCamera" },
                    { 52, null, null, "InspectionContainer" },
                    { 53, null, null, "ReinforcedDrill" },
                    { 54, null, null, "SingleArmIndustrialRobot" },
                    { 55, null, null, "MultiArmIndustrialRobot" },
                    { 30, null, null, "ElectronicMeter" },
                    { 43, null, null, "ContainerTruck" },
                    { 29, null, null, "HammerAndScrewdriver" },
                    { 25, null, null, "MultiTool" },
                    { 27, null, null, "Drill" },
                    { 28, null, null, "ContainerWithWorkshop" },
                    { 1, null, null, "AirHammer" },
                    { 2, null, null, "GravityHammer" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "ItemType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 3, null, null, "SonicHammer" },
                    { 4, null, null, "Excavator" },
                    { 5, null, null, "Chainsaw" },
                    { 6, null, null, "Gravitysaw" },
                    { 7, null, null, "LaserCutter" },
                    { 8, null, null, "Harvester" },
                    { 9, null, null, "HuntingRifle" },
                    { 10, null, null, "RapidFireRifle" },
                    { 12, null, null, "OffRoadVehicleWithMachineGun" },
                    { 11, null, null, "SniperRifle" },
                    { 14, null, null, "SeedlingKit" },
                    { 26, null, null, "ToolBox" },
                    { 24, null, null, "Motorcycle" },
                    { 23, null, null, "NightVision" },
                    { 22, null, null, "TacticalScope" },
                    { 13, null, null, "SeedlingContainer" },
                    { 20, null, null, "ArmoredCarWithMachineGun" },
                    { 21, null, null, "Binoculars" },
                    { 18, null, null, "LargeCaliberRifle" },
                    { 17, null, null, "AutomaticCarbine" },
                    { 16, null, null, "HydroponicContainer" },
                    { 15, null, null, "HydroponicVessel" },
                    { 19, null, null, "LaserRifle" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "LandType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 8, null, null, "Default" },
                    { 7, null, null, "Water" },
                    { 6, null, null, "Ruins" },
                    { 5, null, null, "Urban" },
                    { 1, null, null, "Forrest" },
                    { 3, null, null, "Field" },
                    { 2, null, null, "Mountains" },
                    { 0, null, null, "None" },
                    { 4, null, null, "Farm" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "NpcType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 14, null, null, "Fitter" },
                    { 13, null, null, "Geologist" },
                    { 12, null, null, "Doctor" },
                    { 11, null, null, "Pilot" },
                    { 10, null, null, "Chemist" },
                    { 8, null, null, "Engineer" },
                    { 9, null, null, "Biologist" },
                    { 6, null, null, "Scout" },
                    { 5, null, null, "Soldier" },
                    { 4, null, null, "Farmer" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "NpcType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 3, null, null, "Hunter" },
                    { 2, null, null, "Lumberjack" },
                    { 1, null, null, "Miner" },
                    { 7, null, null, "Mechanic" }
                });

            migrationBuilder.InsertData(
                schema: "dictionary",
                table: "ResourceType",
                columns: new[] { "Id", "CreationDate", "LastModificationDate", "Name" },
                values: new object[,]
                {
                    { 6, null, null, "Meat" },
                    { 1, null, null, "LightWood" },
                    { 2, null, null, "HeavyWood" },
                    { 3, null, null, "IronOre" },
                    { 4, null, null, "TitaniumOre" },
                    { 5, null, null, "Cereal" },
                    { 7, null, null, "Mechanium" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_AttributeTypeId",
                schema: "dbo",
                table: "Attribute",
                column: "AttributeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attribute_NpcId",
                schema: "dbo",
                table: "Attribute",
                column: "NpcId");

            migrationBuilder.CreateIndex(
                name: "IX_Block_BlockTypeId",
                schema: "dbo",
                table: "Block",
                column: "BlockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Building_BuildingTypeId",
                schema: "building",
                table: "Building",
                column: "BuildingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Building_LandId",
                schema: "building",
                table: "Building",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingBonus_BuildingBonusTypeId",
                schema: "building",
                table: "BuildingBonus",
                column: "BuildingBonusTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingBonus_BuildingId",
                schema: "building",
                table: "BuildingBonus",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_GameUser_Email",
                schema: "dbo",
                table: "GameUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameUser_GuildId",
                schema: "dbo",
                table: "GameUser",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GameUser_WalletAddress",
                schema: "dbo",
                table: "GameUser",
                column: "WalletAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guild_Name",
                schema: "guild",
                table: "Guild",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guild_OwnerId",
                schema: "guild",
                table: "Guild",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildBuilding_GuildBuildingTypeId",
                schema: "guild",
                table: "GuildBuilding",
                column: "GuildBuildingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildBuilding_GuildId",
                schema: "guild",
                table: "GuildBuilding",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildBuildingBonus_GuildBuildingBonusTypeId",
                schema: "guild",
                table: "GuildBuildingBonus",
                column: "GuildBuildingBonusTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildBuildingBonus_GuildBuildingId",
                schema: "guild",
                table: "GuildBuildingBonus",
                column: "GuildBuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildImprovement_GuildId",
                schema: "guild",
                table: "GuildImprovement",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildImprovement_GuildImprovementTypeId",
                schema: "guild",
                table: "GuildImprovement",
                column: "GuildImprovementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildInvitation_GuildId",
                schema: "guild",
                table: "GuildInvitation",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildInvitation_UserId_GuildId",
                schema: "guild",
                table: "GuildInvitation",
                columns: new[] { "UserId", "GuildId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildResource_GuildId",
                schema: "guild",
                table: "GuildResource",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildResource_ResourceTypeId",
                schema: "guild",
                table: "GuildResource",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Improvement_ImprovementTypeId",
                schema: "dbo",
                table: "Improvement",
                column: "ImprovementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Improvement_LandId",
                schema: "dbo",
                table: "Improvement",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemTypeId",
                schema: "nft",
                table: "Item",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_UserId",
                schema: "nft",
                table: "Item",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemBonus_ItemBonusTypeId",
                schema: "dbo",
                table: "ItemBonus",
                column: "ItemBonusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemBonus_ItemId",
                schema: "dbo",
                table: "ItemBonus",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_LandId",
                schema: "dbo",
                table: "Job",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_NpcId",
                schema: "dbo",
                table: "Job",
                column: "NpcId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Land_LandInstanceId_PositionX_PositionY",
                schema: "nft",
                table: "Land",
                columns: new[] { "LandInstanceId", "PositionX", "PositionY" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Land_LandTypeId",
                schema: "nft",
                table: "Land",
                column: "LandTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Land_UserId",
                schema: "nft",
                table: "Land",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Email",
                schema: "dbo",
                table: "Notification",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_WalletAddress",
                schema: "dbo",
                table: "Notification",
                column: "WalletAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Npc_BuildingId",
                schema: "nft",
                table: "Npc",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Npc_ItemId",
                schema: "nft",
                table: "Npc",
                column: "ItemId",
                unique: true,
                filter: "[ItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Npc_LandId",
                schema: "nft",
                table: "Npc",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_Npc_NpcTypeId",
                schema: "nft",
                table: "Npc",
                column: "NpcTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Npc_UserId",
                schema: "nft",
                table: "Npc",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_LandFromId",
                schema: "dbo",
                table: "Travel",
                column: "LandFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_LandToId",
                schema: "dbo",
                table: "Travel",
                column: "LandToId");

            migrationBuilder.CreateIndex(
                name: "IX_Travel_NpcId",
                schema: "dbo",
                table: "Travel",
                column: "NpcId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLand_LandId",
                schema: "dbo",
                table: "UserLand",
                column: "LandId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLand_UserId",
                schema: "dbo",
                table: "UserLand",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResource_ResourceTypeId",
                schema: "dbo",
                table: "UserResource",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserResource_UserId",
                schema: "dbo",
                table: "UserResource",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attribute_Npc_NpcId",
                schema: "dbo",
                table: "Attribute",
                column: "NpcId",
                principalSchema: "nft",
                principalTable: "Npc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingBonus_Building_BuildingId",
                schema: "building",
                table: "BuildingBonus",
                column: "BuildingId",
                principalSchema: "building",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Npc_Building_BuildingId",
                schema: "nft",
                table: "Npc",
                column: "BuildingId",
                principalSchema: "building",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Npc_GameUser_UserId",
                schema: "nft",
                table: "Npc",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "GameUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Npc_Item_ItemId",
                schema: "nft",
                table: "Npc",
                column: "ItemId",
                principalSchema: "nft",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Npc_Land_LandId",
                schema: "nft",
                table: "Npc",
                column: "LandId",
                principalSchema: "nft",
                principalTable: "Land",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Building_Land_LandId",
                schema: "building",
                table: "Building",
                column: "LandId",
                principalSchema: "nft",
                principalTable: "Land",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Guild_GameUser_OwnerId",
                schema: "guild",
                table: "Guild",
                column: "OwnerId",
                principalSchema: "dbo",
                principalTable: "GameUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_Guild_GuildId",
                schema: "dbo",
                table: "GameUser");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attribute",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "logs");

            migrationBuilder.DropTable(
                name: "Block",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BuildingBonus",
                schema: "building");

            migrationBuilder.DropTable(
                name: "GuildBuildingBonus",
                schema: "guild");

            migrationBuilder.DropTable(
                name: "GuildImprovement",
                schema: "guild");

            migrationBuilder.DropTable(
                name: "GuildInvitation",
                schema: "guild");

            migrationBuilder.DropTable(
                name: "GuildResource",
                schema: "guild");

            migrationBuilder.DropTable(
                name: "HealthCheck",
                schema: "hc");

            migrationBuilder.DropTable(
                name: "Improvement",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ItemBonus",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Job",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Travel",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserLand",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserResource",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AttributeType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "BlockType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "BuildingBonusType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "GuildBuilding",
                schema: "guild");

            migrationBuilder.DropTable(
                name: "GuildBuildingBonusType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "GuildImprovementType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "ImprovementType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "ItemBonusType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "Npc",
                schema: "nft");

            migrationBuilder.DropTable(
                name: "ResourceType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "GuildBuildingType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "Building",
                schema: "building");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "nft");

            migrationBuilder.DropTable(
                name: "NpcType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "BuildingType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "Land",
                schema: "nft");

            migrationBuilder.DropTable(
                name: "ItemType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "LandInstance",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "LandType",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "Guild",
                schema: "guild");

            migrationBuilder.DropTable(
                name: "GameUser",
                schema: "dbo");
        }
    }
}
