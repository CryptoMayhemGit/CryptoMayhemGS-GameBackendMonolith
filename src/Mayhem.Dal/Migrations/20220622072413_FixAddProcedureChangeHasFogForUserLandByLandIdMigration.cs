using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace Mayhem.Dal.Migrations
{
    public partial class FixAddProcedureChangeHasFogForUserLandByLandIdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop procedure if exists AddFogToLandsProcedure;");

            StringBuilder addFogToLandsProcedureStringBuilder = new();
            addFogToLandsProcedureStringBuilder.AppendLine("CREATE procedure [dbo].[AddFogToLandsProcedure]");
            addFogToLandsProcedureStringBuilder.AppendLine("@LandId bigint,");
            addFogToLandsProcedureStringBuilder.AppendLine("@UserId int,");
            addFogToLandsProcedureStringBuilder.AppendLine("@NpcId bigint");
            addFogToLandsProcedureStringBuilder.AppendLine("as");
            addFogToLandsProcedureStringBuilder.AppendLine("create table #TempTable (Id int, PositionX bigint, PositionY bigint);");
            addFogToLandsProcedureStringBuilder.AppendLine("DECLARE @Id int;");
            addFogToLandsProcedureStringBuilder.AppendLine("DECLARE @PositionX bigint;");
            addFogToLandsProcedureStringBuilder.AppendLine("DECLARE @PositionY bigint;");
            addFogToLandsProcedureStringBuilder.AppendLine("DECLARE db_cursor CURSOR FOR ");
            addFogToLandsProcedureStringBuilder.AppendLine("select l.id, PositionX, PositionY from UserLand as ul ");
            addFogToLandsProcedureStringBuilder.AppendLine("left join nft.land as l on ul.LandId = l.Id ");
            addFogToLandsProcedureStringBuilder.AppendLine("where (UserId = @UserId and Owned = 1 and ul.Status != 1)");
            addFogToLandsProcedureStringBuilder.AppendLine("OPEN db_cursor");
            addFogToLandsProcedureStringBuilder.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            addFogToLandsProcedureStringBuilder.AppendLine("WHILE @@FETCH_STATUS = 0  ");
            addFogToLandsProcedureStringBuilder.AppendLine("BEGIN");
            addFogToLandsProcedureStringBuilder.AppendLine("insert into #TempTable");
            addFogToLandsProcedureStringBuilder.AppendLine("select l.Id, l.PositionX, l.PositionY from UserLand as ul ");
            addFogToLandsProcedureStringBuilder.AppendLine("left join nft.land as l on ul.LandId = l.Id ");
            addFogToLandsProcedureStringBuilder.AppendLine("where");
            addFogToLandsProcedureStringBuilder.AppendLine("(@PositionY % 2 = 1 and");
            addFogToLandsProcedureStringBuilder.AppendLine("((PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY + 1)))");
            addFogToLandsProcedureStringBuilder.AppendLine("or");
            addFogToLandsProcedureStringBuilder.AppendLine("(@PositionY % 2 = 0 and");
            addFogToLandsProcedureStringBuilder.AppendLine("((PositionX = @PositionX - 1 and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY + 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1)));");
            addFogToLandsProcedureStringBuilder.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            addFogToLandsProcedureStringBuilder.AppendLine("END");
            addFogToLandsProcedureStringBuilder.AppendLine("CLOSE db_cursor");
            addFogToLandsProcedureStringBuilder.AppendLine("DEALLOCATE db_cursor");
            addFogToLandsProcedureStringBuilder.AppendLine("DECLARE db_cursor CURSOR FOR ");
            addFogToLandsProcedureStringBuilder.AppendLine("select l.id, PositionX, PositionY from nft.Npc as n ");
            addFogToLandsProcedureStringBuilder.AppendLine("left join nft.land as l on n.LandId = l.Id ");
            addFogToLandsProcedureStringBuilder.AppendLine("where n.UserId = @UserId and n.Id != @NpcId");
            addFogToLandsProcedureStringBuilder.AppendLine("OPEN db_cursor");
            addFogToLandsProcedureStringBuilder.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            addFogToLandsProcedureStringBuilder.AppendLine("WHILE @@FETCH_STATUS = 0 ");
            addFogToLandsProcedureStringBuilder.AppendLine("BEGIN");
            addFogToLandsProcedureStringBuilder.AppendLine("insert into #TempTable");
            addFogToLandsProcedureStringBuilder.AppendLine("select l.Id, l.PositionX, l.PositionY from UserLand as ul");
            addFogToLandsProcedureStringBuilder.AppendLine("left join nft.land as l on ul.LandId = l.Id");
            addFogToLandsProcedureStringBuilder.AppendLine("where");
            addFogToLandsProcedureStringBuilder.AppendLine("(@PositionY % 2 = 1 and");
            addFogToLandsProcedureStringBuilder.AppendLine("((PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY + 1)))");
            addFogToLandsProcedureStringBuilder.AppendLine("or");
            addFogToLandsProcedureStringBuilder.AppendLine("(@PositionY % 2 = 0 and");
            addFogToLandsProcedureStringBuilder.AppendLine("((PositionX = @PositionX - 1 and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY + 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1)));");
            addFogToLandsProcedureStringBuilder.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            addFogToLandsProcedureStringBuilder.AppendLine("END");
            addFogToLandsProcedureStringBuilder.AppendLine("CLOSE db_cursor");
            addFogToLandsProcedureStringBuilder.AppendLine("DEALLOCATE db_cursor");
            addFogToLandsProcedureStringBuilder.AppendLine("select @PositionX = PositionX, @PositionY = PositionY from nft.Land where id = @LandId;");
            addFogToLandsProcedureStringBuilder.AppendLine("update dbo.UserLand set HasFog = 1 where UserId = @UserId and LandId in(");
            addFogToLandsProcedureStringBuilder.AppendLine("select ul.LandId from dbo.UserLand as ul ");
            addFogToLandsProcedureStringBuilder.AppendLine("left join nft.land as l on ul.LandId = l.Id ");
            addFogToLandsProcedureStringBuilder.AppendLine("where ul.UserId = @UserId and (@PositionY % 2 = 1 and");
            addFogToLandsProcedureStringBuilder.AppendLine("((PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY + 1)))");
            addFogToLandsProcedureStringBuilder.AppendLine("or");
            addFogToLandsProcedureStringBuilder.AppendLine("(@PositionY % 2 = 0 and");
            addFogToLandsProcedureStringBuilder.AppendLine("((PositionX = @PositionX - 1 and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY + 1) OR");
            addFogToLandsProcedureStringBuilder.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1)))");
            addFogToLandsProcedureStringBuilder.AppendLine("except");
            addFogToLandsProcedureStringBuilder.AppendLine("select distinct Id from #TempTable)");
            addFogToLandsProcedureStringBuilder.AppendLine("drop table #TempTable;");
            migrationBuilder.Sql(addFogToLandsProcedureStringBuilder.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
