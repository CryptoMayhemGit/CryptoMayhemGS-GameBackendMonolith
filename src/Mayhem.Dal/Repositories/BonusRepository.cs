using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.Guilds;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class BonusRepository : IBonusRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public BonusRepository(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
        }

        public async Task<double> GetMechBonusByUserIdAsync(int userId)
        {
            Tables.GameUser user = await mayhemDataContext
                .GameUsers
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (user != null && user.GuildId != null)
            {
                Guild guild = await mayhemDataContext
                    .Guilds
                    .Include(x => x.GuildBuildings)
                    .ThenInclude(x => x.GuildBuildingBonuses)
                    .SingleOrDefaultAsync(x => x.Id == user.GuildId);

                GuildBuilding fightBoard = guild.GuildBuildings
                    .Where(x => x.GuildBuildingTypeId == GuildBuildingsType.FightBoard)
                    .SingleOrDefault();

                if (fightBoard != null)
                {
                    return fightBoard.GuildBuildingBonuses.Where(x => x.GuildBuildingBonusTypeId == GuildBuildingBonusesType.MechAttack).SingleOrDefault().Bonus;
                }
            }

            return 0;
        }
    }
}
