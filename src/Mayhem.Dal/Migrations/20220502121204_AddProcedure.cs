using Microsoft.EntityFrameworkCore.Migrations;

namespace Mayhem.Dal.Migrations
{
    public partial class AddProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("create procedure ClearOldNotificationProcedure as delete from [dbo].[Notification] where CreationDate < DATEADD(day, -30, GETUTCDATE());");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop procedure ClearOldNotificationProcedure;");
        }
    }
}
