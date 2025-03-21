using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace Mayhem.Dal.Migrations
{
    public partial class ClearOldNotificationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Before you run this migration Make sure you have:
            // 1. In master db login and user ClearOldNotificationCredential
            // use master here
            // create login ClearOldNotificationCredential  with password = 'c9JbkN5m%#sd1YuN$m%';
            // create user ClearOldNotificationCredential from login ClearOldNotificationCredential;
            // end master
            // 2. Created azure elastic job connected to your db

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("begin try");
            stringBuilder.AppendLine("begin tran");
            stringBuilder.AppendLine("create master key encryption by password = '(x_U$~M#<?bAdZ3?mhxq4';");
            stringBuilder.AppendLine("create database scoped credential ClearOldNotificationCredential with identity = 'ClearOldNotificationCredential', secret = 'c9JbkN5m%#sd1YuN$m%';");
            stringBuilder.AppendLine("create user ClearOldNotificationCredential from login ClearOldNotificationCredential;");
            stringBuilder.AppendLine("EXEC sp_addrolemember N'db_accessadmin', N'ClearOldNotificationCredential';");
            stringBuilder.AppendLine("grant execute on schema::dbo to ClearOldNotificationCredential;");
            stringBuilder.AppendLine("exec jobs.sp_add_target_group 'ClearOldNotificationGroup';");
            stringBuilder.AppendLine("exec jobs.sp_add_target_group_member");
            stringBuilder.AppendLine("'ClearOldNotificationGroup',");
            stringBuilder.AppendLine("@target_type = 'SqlServer',");
            stringBuilder.AppendLine("@refresh_credential_name = 'ClearOldNotificationCredential',");
            stringBuilder.AppendLine("@server_name = 'kielson-server.database.windows.net'; ");
            stringBuilder.AppendLine("exec jobs.sp_add_job @job_name = 'ClearOldNotificationJob', @description = 'Remove from table dbo.Notification records older then 30 days';");
            stringBuilder.AppendLine("exec jobs.sp_add_jobstep ");
            stringBuilder.AppendLine("@job_name = 'ClearOldNotificationJob',");
            stringBuilder.AppendLine("@command = N'exec ClearOldNotificationProcedure',");
            stringBuilder.AppendLine("@credential_name = 'ClearOldNotificationCredential',");
            stringBuilder.AppendLine("@target_group_name='ClearOldNotificationGroup'; ");
            stringBuilder.AppendLine("exec jobs.sp_update_job ");
            stringBuilder.AppendLine("@job_name = 'ClearOldNotificationJob',");
            stringBuilder.AppendLine("@enabled = 1,");
            stringBuilder.AppendLine("@schedule_interval_type = 'Days',");
            stringBuilder.AppendLine("@schedule_interval_count = 30; ");
            stringBuilder.AppendLine("exec jobs.sp_start_job 'ClearOldNotificationJob'; ");
            stringBuilder.AppendLine("print 'Operation succeed';");
            stringBuilder.AppendLine("commit tran");
            stringBuilder.AppendLine("end try");
            stringBuilder.AppendLine("begin catch");
            stringBuilder.AppendLine("print 'Operation failed';");
            stringBuilder.AppendLine("rollback tran;");
            stringBuilder.AppendLine("end catch");
            migrationBuilder.Sql(stringBuilder.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
