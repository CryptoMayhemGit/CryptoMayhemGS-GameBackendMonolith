using AutoMapper;
using Mayhem.Dal.Dto.Classes.Attributes;
using Mayhem.Dal.Dto.Classes.Buildings;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Buildings;
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
    public class BuildingRepository : IBuildingRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public BuildingRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<BuildingDto> AddBuildingToLandAsync(long landId, BuildingsType buildingType, int userId)
        {
            Land land = await mayhemDataContext
                .Lands
                .Include(x => x.Jobs)
                .SingleOrDefaultAsync(x => x.Id == landId);

            Building building = new()
            {
                LandId = landId,
                BuildingTypeId = buildingType,
                Level = 1,
                BuildingBonuses = new List<BuildingBonus>()
                {
                    new BuildingBonus()
                    {
                        BuildingBonusTypeId = BuildingBonusDictionary.GetBuildingBonusesType(buildingType),
                        Bonus = BuildingBonusDictionary.GetBuildingBonusValues(buildingType, 1),
                    }
                }
            };

            Dictionary<ResourcesType, int> costs = BuildingCostsDictionary.GetBuildingCosts(buildingType, 1);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                List<UserResource> resources = await mayhemDataContext
                    .UserResources
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                foreach (KeyValuePair<ResourcesType, int> cost in costs)
                {
                    UserResource resource = resources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                    resource.Value -= cost.Value;
                }
                EntityEntry<Building> addedBuilding = await mayhemDataContext.Buildings.AddAsync(building);

                Job job = land.Jobs.First();

                foreach (BuildingBonus buildingBonus in building.BuildingBonuses)
                {
                    IEnumerable<AttributesType> attributes = AttributeBonusDictionary.GetAttributeTypesByBuildingBonusType(buildingBonus.BuildingBonusTypeId);
                    List<Tables.Attribute> attributesToUpdate = job.Npc.Attributes.Where(x => attributes.Contains(x.AttributeTypeId)).ToList();
                    foreach (Tables.Attribute attribute in attributesToUpdate)
                    {
                        attribute.Value = BonusHelper.IncreaseBonusValue(buildingBonus.Bonus, attribute.BaseValue, attribute.Value);
                    }
                }
                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();

                return mapper.Map<BuildingDto>(addedBuilding.Entity);
            }
            catch (ValidationException)
            {
                await ts.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(AddBuildingToLandAsync));
            }
        }

        public async Task<BuildingDto> UpgradeBuildingAsync(long buildingId, int userId)
        {
            Building building = await mayhemDataContext
            .Buildings
            .Include(x => x.BuildingBonuses)
            .Include(x => x.Land)
            .ThenInclude(x => x.Jobs)
            .SingleOrDefaultAsync(x => x.Id == buildingId);

            Dictionary<ResourcesType, int> costs = BuildingCostsDictionary.GetBuildingCosts(building.BuildingTypeId, building.Level + 1);

            List<UserResource> resources = await mayhemDataContext
                .UserResources
                .Where(x => x.UserId == userId)
                .ToListAsync();

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                foreach (KeyValuePair<ResourcesType, int> cost in costs)
                {
                    UserResource resource = resources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                    resource.Value -= cost.Value;
                }

                building.Level++;
                BuildingBonus bonus = building.BuildingBonuses.First();
                bonus.Bonus = BuildingBonusDictionary.GetBuildingBonusValues(building.BuildingTypeId, building.Level);

                Job job = building.Land.Jobs.First();

                foreach (BuildingBonus buildingBonus in building.BuildingBonuses)
                {
                    IEnumerable<AttributesType> attributes = AttributeBonusDictionary.GetAttributeTypesByBuildingBonusType(buildingBonus.BuildingBonusTypeId);
                    List<Tables.Attribute> attributesToUpdate = job.Npc.Attributes.Where(x => attributes.Contains(x.AttributeTypeId)).ToList();
                    foreach (Tables.Attribute attribute in attributesToUpdate)
                    {
                        attribute.Value = BonusHelper.IncreaseBonusValue(buildingBonus.Bonus, attribute.BaseValue, attribute.Value);
                    }
                }

                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();

                return mapper.Map<BuildingDto>(building);
            }
            catch (ValidationException)
            {
                await ts.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(UpgradeBuildingAsync));
            }
        }

        public async Task<IEnumerable<BuildingDto>> GetBuildingsByLandIdAsync(long landId) => await mayhemDataContext
                .Buildings
                .AsNoTracking()
                .Where(x => x.LandId == landId)
                .Select(x => mapper.Map<BuildingDto>(x))
                .ToListAsync();

        public async Task<BuildingDto> GetBuildingByIdAsync(long buildingId) => await mayhemDataContext
                .Buildings
                .AsNoTracking()
                .Where(x => x.Id == buildingId)
                .Select(x => mapper.Map<BuildingDto>(x))
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<BuildingDto>> GetBuildingsByUserIdAsync(int userId)
        {
            List<Building> buildings = await mayhemDataContext
                .Lands
                .Include(x => x.UserLands.Where(x => x.UserId == userId && x.Owned))
                .Include(x => x.Buildings)
                .AsNoTracking()
                .SelectMany(x => x.Buildings)
                .ToListAsync();

            return buildings.Select(x => mapper.Map<BuildingDto>(x));
        }

        public async Task<IEnumerable<BuildingDto>> GetEnemyBuildingsByUserIdAsync(int userId, IEnumerable<long> landIds)
        {
            List<Building> buildings = await mayhemDataContext
                .Lands
                .Include(x => x.UserLands.Where(x => x.UserId != userId && x.Owned))
                .Include(x => x.Buildings)
                .AsNoTracking()
                .Where(x => landIds.Contains(x.Id))
                .SelectMany(x => x.Buildings)
                .ToListAsync();

            return buildings.Select(x => mapper.Map<BuildingDto>(x));
        }

        public async Task<LandDto> GetLandByBuildingIdAsync(long buildingId)
        {
            return await mayhemDataContext
                .Buildings
                .Include(x => x.Land)
                .Where(x => x.Id == buildingId)
                .Select(x => mapper.Map<LandDto>(x.Land))
                .SingleOrDefaultAsync();
        }
    }
}
