using AutoMapper;
using Dapper;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Helpers;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.SqlDapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class UserLandRepository : IUserLandRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public UserLandRepository(IMayhemDataContext mayhemDataContext, IMapper mapper, IMayhemConfigurationService mayhemConfigurationService)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
            this.mayhemConfigurationService = mayhemConfigurationService;
        }

        public async Task<UserLandDto> GetUserLandAsync(long userLandId) => await mayhemDataContext
            .UserLands
            .Include(x => x.Land)
            .AsNoTracking()
            .Where(x => x.Id == userLandId)
            .Select(x => mapper.Map<UserLandDto>(x))
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<UserLandDto>> GetUserLandsAsync(int userId) => await mayhemDataContext
                .UserLands
                .Include(x => x.Land)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => mapper.Map<UserLandDto>(x))
                .ToListAsync();

        public async Task<IEnumerable<UserLandDto>> GetEnemyUserLandsAsync(int userId, IEnumerable<long> landIds) => await mayhemDataContext
                .UserLands
                .Include(x => x.Land)
                .AsNoTracking()
                .Where(x => x.UserId != userId && landIds.Contains(x.Id) && x.Owned)
                .Select(x => mapper.Map<UserLandDto>(x))
                .ToListAsync();

        public async Task<IEnumerable<LandPositionDto>> GetUserLandsFromUserPerspectiveAsync(int userId, int landInstanceId)
        {
            using (IDbConnection db = new SqlConnection(mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString))
            {
                return await db.QueryAsync<LandPositionDto>(SqlQuerries.GetUserLandsFromUserPerspectiveSql, new { UserId = userId });
            }
        }

        public async Task<UserLandDto> DiscoverUserLandAsync(long landId, int userId)
        {
            UserLand userLand = await mayhemDataContext
                .UserLands
                .Where(x => x.UserId == userId && x.LandId == landId)
                .SingleOrDefaultAsync();

            if (userLand == null)
            {
                userLand = new()
                {
                    UserId = userId,
                    LandId = landId,
                    Status = LandsStatus.Discovered,
                    HasFog = false,
                    Owned = false,
                };
                await mayhemDataContext.UserLands.AddAsync(userLand);
            }
            else
            {
                userLand.Status = LandsStatus.Discovered;
                userLand.HasFog = false;
                await ChangeFogForNeighboursAsync(userId, userLand);
            }

            await mayhemDataContext.SaveChangesAsync();
            return mapper.Map<UserLandDto>(userLand);
        }

        public async Task<UserLandDto> ExploreUserLandAsync(long landId, int userId)
        {
            UserLand userLand = await mayhemDataContext
                .UserLands
                .Where(x => x.UserId == userId && x.LandId == landId)
                .SingleAsync();

            userLand.Status = LandsStatus.Explored;

            await mayhemDataContext.SaveChangesAsync();

            return mapper.Map<UserLandDto>(userLand);
        }

        private async Task ChangeFogForNeighboursAsync(int userId, UserLand userLand)
        {
            if (userLand.Owned)
            {
                List<UserLand> neighboringUserLands = await mayhemDataContext
                .UserLands
                .Include(x => x.Land)
                .Where(EntityHelper.GetNeighboursByUserLand(userId, userLand.Land.PositionX, userLand.Land.PositionY))
                .ToListAsync();

                foreach (UserLand neighboringUserLand in neighboringUserLands)
                {
                    neighboringUserLand.HasFog = false;
                }
            }
        }

        public async Task<bool> CheckPurchaseLandAsync(long landId, int userId)
        {
            List<UserLand> list = await mayhemDataContext
                .UserLands
                .AsNoTracking()
                .Where(x => x.LandId == landId)
                .ToListAsync();

            return (list.Any() && ((list.All(x => x.UserId == userId && x.Status == LandsStatus.Explored && !x.Owned)) 
                || (list.Any(x => x.UserId != userId && x.Owned && x.OnSale))));
        }
    }
}
