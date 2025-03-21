using AutoMapper;
using Mayhem.Dal.Dto.Classes.Attributes;
using Mayhem.Dal.Dto.Classes.GuildBuildings;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Helper;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class GuildBuildingRepository : IGuildBuildingRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public GuildBuildingRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<GuildBuildingDto> AddGuildBuildingAsync(int guildId, GuildBuildingsType guildBuildingsType, int userId)
        {
            Guild guild = await mayhemDataContext
                .Guilds
                .Include(x => x.Users)
                .ThenInclude(x => x.Npcs)
                .ThenInclude(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == guildId);

            GuildBuilding guildBuilding = new()
            {
                Level = 1,
                GuildId = guild.Id,
                GuildBuildingTypeId = guildBuildingsType,

                GuildBuildingBonuses = new List<GuildBuildingBonus>()
                {
                    new GuildBuildingBonus()
                    {
                        GuildBuildingBonusTypeId = GuildBuildingBonusDictionary.GetBuildingBonusesType(guildBuildingsType),
                        Bonus = GuildBuildingBonusDictionary.GetGuildBuildingBonusValues(guildBuildingsType, 1)
                    }
                }
            };

            Dictionary<ResourcesType, int> costs = GuildBuildingCostsDictionary.GetGuildBuildingCosts(guildBuildingsType, 1);
            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                List<GuildResource> resources = await mayhemDataContext
                    .GuildResources
                    .Where(x => x.GuildId == guildId)
                    .ToListAsync();

                foreach (KeyValuePair<ResourcesType, int> cost in costs)
                {
                    GuildResource resource = resources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                    resource.Value -= cost.Value;
                }

                foreach (GameUser user in guild.Users)
                {
                    UpdateUserNpcsAttributes(user, guildBuilding.GuildBuildingTypeId, GuildBuildingBonusDictionary.GetGuildBuildingBonusValues(guildBuildingsType, 1));
                }

                EntityEntry<GuildBuilding> addedBuilding = await mayhemDataContext.GuildBuildings.AddAsync(guildBuilding);
                await mayhemDataContext.SaveChangesAsync();

                await ts.CommitAsync();

                return mapper.Map<GuildBuildingDto>(addedBuilding.Entity);
            }
            catch (ValidationException)
            {
                await ts.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(AddGuildBuildingAsync));
            }
        }

        public async Task<GuildBuildingDto> UpgradeGuildBuildingAsync(int buildingId, int userId)
        {
            GuildBuilding guildBuilding = await mayhemDataContext
            .GuildBuildings
            .Include(x => x.Guild)
            .ThenInclude(x => x.Users)
            .ThenInclude(x => x.Npcs)
            .ThenInclude(x => x.Attributes)
            .Include(x => x.GuildBuildingBonuses)
            .SingleOrDefaultAsync(x => x.Id == buildingId);

            Dictionary<ResourcesType, int> costs = GuildBuildingCostsDictionary.GetGuildBuildingCosts(guildBuilding.GuildBuildingTypeId, guildBuilding.Level + 1);

            List<GuildResource> resources = await mayhemDataContext
                .GuildResources
                .Include(x => x.Guild)
                .ThenInclude(x => x.Users)
                .ThenInclude(x => x.Npcs)
                .Where(x => x.GuildId == guildBuilding.Guild.Id)
                .ToListAsync();

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                foreach (KeyValuePair<ResourcesType, int> cost in costs)
                {
                    GuildResource resource = resources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                    resource.Value -= cost.Value;
                }

                guildBuilding.Level++;
                GuildBuildingBonus bonus = guildBuilding.GuildBuildingBonuses.First();
                if (guildBuilding.Level > 3)
                {
                    bonus.Bonus += GuildBuildingBonusDictionary.GetGuildBuildingBonusValues(guildBuilding.GuildBuildingTypeId, guildBuilding.Level);
                }
                else
                {
                    bonus.Bonus = GuildBuildingBonusDictionary.GetGuildBuildingBonusValues(guildBuilding.GuildBuildingTypeId, guildBuilding.Level);
                }

                foreach (GameUser user in guildBuilding.Guild.Users)
                {
                    UpdateUserNpcsAttributes(user, guildBuilding.GuildBuildingTypeId, GuildBuildingBonusDictionary.GetGuildBuildingBonusValues(guildBuilding.GuildBuildingTypeId, guildBuilding.Level));
                }

                await mayhemDataContext.SaveChangesAsync();

                await ts.CommitAsync();

                return mapper.Map<GuildBuildingDto>(guildBuilding);
            }
            catch (ValidationException)
            {
                await ts.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(UpgradeGuildBuildingAsync));
            }
        }

        public async Task<IEnumerable<GuildBuildingDto>> GetGuildBuildingsByGuildIdAsync(int guildId) => await mayhemDataContext
                .GuildBuildings
                .AsNoTracking()
                .Where(x => x.GuildId == guildId)
                .Select(x => mapper.Map<GuildBuildingDto>(x))
                .ToListAsync();

        public async Task<GuildBuildingDto> GetGuildBuildingByIdAsync(int guildBuildingId) => await mayhemDataContext
                .GuildBuildings
                .AsNoTracking()
                .Where(x => x.Id == guildBuildingId)
                .Select(x => mapper.Map<GuildBuildingDto>(x))
                .SingleOrDefaultAsync();

        private static void UpdateUserNpcsAttributes(GameUser user, GuildBuildingsType guildBuildingsType, double bonus)
        {
            foreach (Npc npc in user.Npcs)
            {
                IEnumerable<AttributesType> attributes = AttributeBonusDictionary.GetAttributeTypesByGuildBuildingType(guildBuildingsType);
                List<Tables.Attribute> attributesToUpdate = npc.Attributes.Where(x => attributes.Contains(x.AttributeTypeId)).ToList();
                foreach (Tables.Attribute attribute in attributesToUpdate)
                {
                    attribute.Value = BonusHelper.IncreaseBonusValue(bonus, attribute.BaseValue, attribute.Value);
                }
            }
        }
    }
}
