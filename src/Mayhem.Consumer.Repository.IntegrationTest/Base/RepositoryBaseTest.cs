using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Consumer.Dal.Interfaces.Wrapers;
using Mayhem.Consumer.Dal.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Mayhem.Consumer.IntegrationTest.Base
{
    internal class RepositoryBaseTest
    {
        public const string ConnectionString = "Server=tcp:kielson-server.database.windows.net,1433;Initial Catalog=Mayhem-TestDb;Persist Security Info=False;User ID=kielson;Password=SuperHaslo123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        protected IMayhemConfigurationService GetMayhemConfigurationService()
        {
            return new MayhemConfigurationService(new MayhemConfiguration()
            {
                ConnectionStringsConfigruation = new ConnectionStringsConfigruation()
                {
                    MSSQLConnectionString = ConnectionString,
                }
            });
        }

        protected INpcRepository GetNpcRepository()
        {
            return new NpcRepository(GetMayhemConfigurationService(), new DapperWrapper(), new Mock<ILogger<NpcRepository>>().Object);
        }

        protected IItemRepository GetItemRepository()
        {
            return new ItemRepository(GetMayhemConfigurationService(), new DapperWrapper(), new Mock<ILogger<ItemRepository>>().Object);
        }

        protected ILandRepository GetLandRepository()
        {
            return new LandRepository(GetMayhemConfigurationService(), new DapperWrapper(), new Mock<ILogger<LandRepository>>().Object);
        }
    }
}
