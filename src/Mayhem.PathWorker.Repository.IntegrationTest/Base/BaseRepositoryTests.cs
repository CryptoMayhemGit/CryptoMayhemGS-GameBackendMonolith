using Dapper;
using Mayhem.Common.Services.PathFindingService.Implementations;
using Mayhem.Common.Services.PathFindingService.Interfaces;
using Mayhem.Configuration.Interfaces;
using Mayhem.Configuration.Services;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Mayhem.Workers.Dal.Repositories.Services;
using NUnit.Framework;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.PathWorker.Repository.IntegrationTest.Base
{
    internal class BaseRepositoryTests
    {
        public const string ConnectionString = "Server=tcp:kielson-server.database.windows.net,1433;Initial Catalog=Mayhem-TestDb;Persist Security Info=False;User ID=kielson;Password=SuperHaslo123!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        protected IMayhemConfigurationService MayhemConfigurationService => new MayhemConfigurationService(new MayhemConfiguration()
        {
            ConnectionStringsConfigruation = new ConnectionStringsConfigruation()
            {
                MSSQLConnectionString = ConnectionString,
            },
            CommonConfiguration = new CommonConfiguration()
            {
                ResendNotificationOlderThenInMinutes = 10,
                PlanetSize = 8
            },
        });

        protected INpcRepository GetNpcRepository()
        {
            return new NpcRepository(MayhemConfigurationService);
        }

        protected IBlockRepository GetBlockRepository()
        {
            return new BlockRepository(MayhemConfigurationService);
        }

        protected ILandRepository GetLandRepository()
        {
            return new LandRepository(MayhemConfigurationService);
        }

        protected INotificationRepository GetNotificationRepository()
        {
            return new NotificationRepository(MayhemConfigurationService);
        }

        protected ITravelRepository GetTravelRepository()
        {
            return new TravelRepository(MayhemConfigurationService);
        }

        protected IUserLandRepository GetUserLandRepository()
        {
            return new UserLandRepository(MayhemConfigurationService);
        }

        protected IPathFindingService GetPathFindingService()
        {
            return new PathFindingService();
        }

        protected IDiscoveryMissionRepository GetDiscoveryMissionRepository()
        {
            return new DiscoveryMissionRepository(MayhemConfigurationService);
        }

        protected IExploreMissionRepository GetExploreMissionRepository()
        {
            return new ExploreMissionRepository(MayhemConfigurationService);
        }

        [TearDown]
        public async Task CleanUp()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                await db.QueryAsync("delete from [mission].[DiscoveryMission];");
                await db.QueryAsync("DBCC CHECKIDENT ('[mission].[DiscoveryMission]', RESEED, 0);");
                await db.QueryAsync("delete from [mission].[ExploreMission];");
                await db.QueryAsync("DBCC CHECKIDENT ('[mission].[ExploreMission]', RESEED, 0);");
                await db.QueryAsync("delete from [dbo].[Travel];");
                await db.QueryAsync("DBCC CHECKIDENT ('[dbo].[Travel]', RESEED, 0);");
                await db.QueryAsync("delete from [dbo].[UserLand];");
                await db.QueryAsync("DBCC CHECKIDENT ('[dbo].[UserLand]', RESEED, 0);");
                await db.QueryAsync("delete from [nft].[Npc];");
                await db.QueryAsync("DBCC CHECKIDENT ('[nft].[Npc]', RESEED, 0);");
                await db.QueryAsync("delete from [nft].[Land];");
                await db.QueryAsync("DBCC CHECKIDENT ('[nft].[Land]', RESEED, 0);");
                await db.QueryAsync("delete from [dbo].[LandInstance];");
                await db.QueryAsync("DBCC CHECKIDENT ('[dbo].[LandInstance]', RESEED, 0);");
                await db.QueryAsync("delete from [dbo].[GameUser];");
                await db.QueryAsync("DBCC CHECKIDENT ('[dbo].[GameUser]', RESEED, 0);");
            }
        }
    }
}
