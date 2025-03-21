using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace Mayhem.Dal.Migrations
{
    public partial class AddProcedureChangeHasFogForUserLandByLandIdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            StringBuilder stringBuilder1 = new();
            stringBuilder1.AppendLine("create procedure ChangeHasFogAsTrueForUserLandByLandIdProcedure");
            stringBuilder1.AppendLine("@LandId bigint,");
            stringBuilder1.AppendLine("@UserId int");
            stringBuilder1.AppendLine("as");
            stringBuilder1.AppendLine("DECLARE @PositionX bigint;");
            stringBuilder1.AppendLine("DECLARE @PositionY bigint;");
            stringBuilder1.AppendLine("select @PositionX = PositionX, @PositionY = PositionY from nft.Land where id = @LandId;");
            stringBuilder1.AppendLine("update dbo.UserLand set HasFog = 1 where id in(");
            stringBuilder1.AppendLine("select ul.Id from dbo.UserLand as ul ");
            stringBuilder1.AppendLine("left join nft.land as l on ul.LandId = l.Id ");
            stringBuilder1.AppendLine("where ul.UserId = @UserId and (");
            stringBuilder1.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY - 1) OR ");
            stringBuilder1.AppendLine("(PositionX = @PositionX and PositionY = @PositionY - 1) OR ");
            stringBuilder1.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR ");
            stringBuilder1.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR ");
            stringBuilder1.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR ");
            stringBuilder1.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY + 1) OR ");
            stringBuilder1.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1)));");
            migrationBuilder.Sql(stringBuilder1.ToString());

            StringBuilder stringBuilder2 = new();
            stringBuilder2.AppendLine("create procedure ChangeHasFogAsFalseForUserLandByLandIdProcedure");
            stringBuilder2.AppendLine("@LandId bigint,");
            stringBuilder2.AppendLine("@UserId int");
            stringBuilder2.AppendLine("as");
            stringBuilder2.AppendLine("create table #TempTable (Id int, PositionX bigint, PositionY bigint);");
            stringBuilder2.AppendLine("DECLARE @Id int;");
            stringBuilder2.AppendLine("DECLARE @PositionX bigint;");
            stringBuilder2.AppendLine("DECLARE @PositionY bigint;");
            stringBuilder2.AppendLine("DECLARE db_cursor CURSOR FOR ");
            stringBuilder2.AppendLine("select l.id, PositionX, PositionY from UserLand as ul ");
            stringBuilder2.AppendLine("left join nft.land as l on ul.LandId = l.Id ");
            stringBuilder2.AppendLine("where (UserId = @UserId and Owned = 1) ");
            stringBuilder2.AppendLine("OPEN db_cursor ");
            stringBuilder2.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            stringBuilder2.AppendLine("WHILE @@FETCH_STATUS = 0  ");
            stringBuilder2.AppendLine("BEGIN ");
            stringBuilder2.AppendLine("insert into #TempTable");
            stringBuilder2.AppendLine("select l.Id, l.PositionX, l.PositionY from UserLand as ul ");
            stringBuilder2.AppendLine("left join nft.land as l on ul.LandId = l.Id ");
            stringBuilder2.AppendLine("where");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY - 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY + 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1);");
            stringBuilder2.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            stringBuilder2.AppendLine("END");
            stringBuilder2.AppendLine("CLOSE db_cursor ");
            stringBuilder2.AppendLine("DEALLOCATE db_cursor ");
            stringBuilder2.AppendLine("DECLARE db_cursor CURSOR FOR ");
            stringBuilder2.AppendLine("select l.id, PositionX, PositionY from nft.Npc as n ");
            stringBuilder2.AppendLine("left join nft.land as l on n.LandId = l.Id ");
            stringBuilder2.AppendLine("where n.UserId = @UserId");
            stringBuilder2.AppendLine("OPEN db_cursor ");
            stringBuilder2.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            stringBuilder2.AppendLine("WHILE @@FETCH_STATUS = 0 ");
            stringBuilder2.AppendLine("BEGIN");
            stringBuilder2.AppendLine("insert into #TempTable");
            stringBuilder2.AppendLine("select l.Id, l.PositionX, l.PositionY from UserLand as ul");
            stringBuilder2.AppendLine("left join nft.land as l on ul.LandId = l.Id");
            stringBuilder2.AppendLine("where");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY - 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY + 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1);");
            stringBuilder2.AppendLine("FETCH NEXT FROM db_cursor INTO @Id, @PositionX, @PositionY");
            stringBuilder2.AppendLine("END");
            stringBuilder2.AppendLine("CLOSE db_cursor ");
            stringBuilder2.AppendLine("DEALLOCATE db_cursor");
            stringBuilder2.AppendLine("select @PositionX = PositionX, @PositionY = PositionY from nft.Land where id = @LandId;");
            stringBuilder2.AppendLine("update dbo.UserLand set HasFog = 0 where id in(");
            stringBuilder2.AppendLine("select ul.Id from dbo.UserLand as ul ");
            stringBuilder2.AppendLine("left join nft.land as l on ul.LandId = l.Id ");
            stringBuilder2.AppendLine("where ul.UserId = @UserId and (");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY - 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY - 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX + 1 and PositionY = @PositionY) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX - 1 and PositionY = @PositionY + 1) OR");
            stringBuilder2.AppendLine("(PositionX = @PositionX and PositionY = @PositionY + 1))");
            stringBuilder2.AppendLine("except");
            stringBuilder2.AppendLine("select distinct Id from #TempTable)");
            stringBuilder2.AppendLine("drop table #TempTable;");
            migrationBuilder.Sql(stringBuilder2.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop procedure ChangeHasFogAsTrueForUserLandByLandIdProcedure;");
            migrationBuilder.Sql("drop procedure ChangeHasFogAsFalseForUserLandByLandIdProcedure;");
        }
    }
}
