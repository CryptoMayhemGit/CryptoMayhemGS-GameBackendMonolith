using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class BuildingRepositoryTests : UnitTestBase
    {
        private IBuildingRepository buildingRepository;
        private IMayhemDataContext mayhemDataContext;
        private int userId;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            buildingRepository = GetService<IBuildingRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();

            userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            })).Entity.Id;

            await mayhemDataContext.SaveChangesAsync();
        }

        [Test]
        public async Task GetBuildingsByLandId_WhenBuildingsExists_ThenGetThem_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.Buildings.AddAsync(new Building()
            {
                LandId = newLand.Entity.Id,
            });
            await mayhemDataContext.Buildings.AddAsync(new Building()
            {
                LandId = newLand.Entity.Id
            });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<BuildingDto> buildings = await buildingRepository.GetBuildingsByLandIdAsync(newLand.Entity.Id);

            buildings.Should().NotBeNull();
            buildings.Should().HaveCount(2);
        }

        public async Task AddBuildingToLand_WhenBuildingAdded_ThenGetIt_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Field,
            });
            await mayhemDataContext.SaveChangesAsync();

            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.DroneFactory, userId);

            building.Should().NotBeNull();
            building.Level.Should().Be(1);
            building.LandId.Should().Be(newLand.Entity.Id);
            building.BuildingTypeId.Should().Be(BuildingsType.DroneFactory);
        }

        public async Task GetBuildingById_WhenBuildingExist_ThenGetIt_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Field,
            });
            await mayhemDataContext.SaveChangesAsync();

            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.DroneFactory, userId);

            BuildingDto buildingDb = await buildingRepository.GetBuildingByIdAsync(building.Id);

            buildingDb.Should().NotBeNull();
        }

        public async Task UpgradeBuilding_WhenBuildingUpgraded_ThenGetIt_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Forest,
            });
            await mayhemDataContext.SaveChangesAsync();

            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.OreMine, userId);
            BuildingDto upgradedBuilding = await buildingRepository.UpgradeBuildingAsync(building.Id, userId);

            upgradedBuilding.Should().NotBeNull();
            upgradedBuilding.Level.Should().Be(building.Level + 1);
            upgradedBuilding.BuildingBonuses.Should().HaveCount(1);
            upgradedBuilding.BuildingBonuses.First().Bonus.Should().NotBe(building.BuildingBonuses.First().Bonus);
        }

        public async Task UpgradeBuildingManyTimes_WhenBuildingUpgraded_ThenGetIt_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(100000),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Mountain,
            });
            await mayhemDataContext.SaveChangesAsync();

            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.Guardhouse, user.Entity.Id);
            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);
            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);
            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);
            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);
            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);
            BuildingDto upgradedBuilding = await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);

            upgradedBuilding.Should().NotBeNull();
            upgradedBuilding.Level.Should().Be(7);
            upgradedBuilding.BuildingBonuses.Should().HaveCount(1);
            Math.Round(upgradedBuilding.BuildingBonuses.First().Bonus, 1).Should().Be(24);
        }

        [Test]
        public async Task AddBuilding_WhenBuildingAdded_ThenIncreaseNpcAttributes_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(100000),
            });
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Swamp,
                Jobs = new List<Job>()
                {
                    new Job()
                    {
                        NpcId = npc.Entity.Id,
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            List<Dal.Tables.Attribute> attributes = (await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == npc.Entity.Id)).Attributes.ToList();
            double lightWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.Lumbermill, user.Entity.Id);

            lightWoodProductionBefore.Should().Be(1.5);
            heavyWoodProductionBefore.Should().Be(0.8);
            attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(1.515);
            attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.808);

        }

        [Test]
        public async Task GetLandByBuildingId_WhenBuildingAndLandExist_ThenGetId_Test()
        {
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                Buildings = new List<Building>()
                {
                    new Building()
                }
            });

            await mayhemDataContext.SaveChangesAsync();

            LandDto landDb = await buildingRepository.GetLandByBuildingIdAsync(land.Entity.Buildings.First().Id);

            landDb.Should().NotBeNull();
            landDb.Id.Should().Be(land.Entity.Id);
        }

        [Test]
        public async Task GetLandByBuildingId_WhenBuildingNotExist_ThenGetNull_Test()
        {
            LandDto landDb = await buildingRepository.GetLandByBuildingIdAsync(102);

            landDb.Should().BeNull();
        }

        [Test]
        public async Task GetLandByBuildingId_WhenLandNotExist_ThenGetNull_Test()
        {
            EntityEntry<Building> building = await mayhemDataContext.Buildings.AddAsync(new Building());
            await mayhemDataContext.SaveChangesAsync();

            LandDto landDb = await buildingRepository.GetLandByBuildingIdAsync(building.Entity.Id);

            landDb.Should().BeNull();
        }

        [Test]
        public async Task UpgradeBuildingManyTime_WhenBuildingUpgraded_ThenIncreaseNpcAttributes_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(100000),
            });
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                NpcTypeId = NpcsType.Lumberjack,
                Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Swamp,
                Jobs = new List<Job>()
                {
                    new Job()
                    {
                        NpcId = npc.Entity.Id,
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            List<Dal.Tables.Attribute> attributes = (await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == npc.Entity.Id)).Attributes.ToList();
            double lightWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionBefore = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.Lumbermill, user.Entity.Id);

            double lightWoodProductionLevel1 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel1 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);

            double lightWoodProductionLevel2 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel2 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);

            double lightWoodProductionLevel3 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel3 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);

            double lightWoodProductionLevel4 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel4 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);

            double lightWoodProductionLevel5 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel5 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);

            double lightWoodProductionLevel6 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel6 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;

            await buildingRepository.UpgradeBuildingAsync(building.Id, user.Entity.Id);

            double lightWoodProductionLevel7 = attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value;
            double heavyWoodProductionLevel7 = attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value;


            lightWoodProductionBefore.Should().Be(1.5);
            heavyWoodProductionBefore.Should().Be(0.8);
            lightWoodProductionLevel1.Should().Be(1.515);
            heavyWoodProductionLevel1.Should().Be(0.808);
            lightWoodProductionLevel2.Should().Be(1.545);
            heavyWoodProductionLevel2.Should().Be(0.824);
            lightWoodProductionLevel3.Should().Be(1.59);
            heavyWoodProductionLevel3.Should().Be(0.848);
            lightWoodProductionLevel4.Should().Be(1.6365);
            heavyWoodProductionLevel4.Should().Be(0.8728);
            lightWoodProductionLevel5.Should().Be(1.6845);
            heavyWoodProductionLevel5.Should().Be(0.8984);
            lightWoodProductionLevel6.Should().Be(1.734);
            heavyWoodProductionLevel6.Should().Be(0.9248);
            lightWoodProductionLevel7.Should().Be(1.785);
            heavyWoodProductionLevel7.Should().Be(0.952);
        }
    }
}
