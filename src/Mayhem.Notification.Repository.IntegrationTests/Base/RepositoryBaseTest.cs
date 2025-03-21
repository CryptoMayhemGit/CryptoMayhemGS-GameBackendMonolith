using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Mayhem.Workers.Dal.Repositories.Services;

namespace Mayhem.Notification.Repository.IntegrationTests.Base
{
    public class RepositoryBaseTest
    {
        public const string ConnectionString = "Server=tcp:kielson-server.database.windows.net,1433;Initial Catalog=Mayhem-TestDb;Persist Security Info=False;User ID=kielson;Password=SuperHaslo123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        protected IMayhemConfigurationService GetMayhemConfigurationService()
        {
            return new MayhemConfigurationService(new MayhemConfiguration()
            {
                ConnectionStringsConfigruation = new ConnectionStringsConfigruation()
                {
                    MSSQLConnectionString = ConnectionString,
                },
                CommonConfiguration = new CommonConfiguration()
                {
                    ResendNotificationOlderThenInMinutes = 1,
                }
            });
        }

        protected INotificationRepository GetNotificationRepository()
        {
            return new NotificationRepository(GetMayhemConfigurationService());
        }

        protected IBlockRepository GetBlockRepository()
        {
            return new BlockRepository(GetMayhemConfigurationService());
        }
    }
}
