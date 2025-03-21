using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.AddBuildingToLand;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class AddBuildingToLandCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IBuildingRepository buildingRepository;
        private ICostsValidationService costsValidationService;

        [OneTimeSetUp]
        public void SetUp()
        {
            buildingRepository = GetService<IBuildingRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
            costsValidationService = GetService<ICostsValidationService>();
        }

        public async Task AddBuildingToLand_WhenBuildingAlredyExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(10000),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Field,
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = user.Entity.Id,
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.DroneFactory, user.Entity.Id);

            AddBuildingToLandCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddBuildingToLandCommandRequest()
            {
                BuildingTypeId = BuildingsType.DroneFactory,
                UserId = user.Entity.Id,
                LandId = newLand.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {newLand.Entity.Id} already has a building.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        public async Task AddBuildingToLand_WhenBuildingHasWrongType_ThenGetValidationErrors_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(10000),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Biome1,
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = user.Entity.Id,
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            AddBuildingToLandCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddBuildingToLandCommandRequest()
            {
                BuildingTypeId = BuildingsType.DroneFactory,
                UserId = user.Entity.Id,
                LandId = newLand.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Building DroneFactory cannot be build on land Ruins.");
            result.Errors.First().PropertyName.Should().Be("BuildingId");
        }

        public async Task AddBuildingToLand_WhenLandIsUndiscover_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(10000),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Desert,
            });
            await mayhemDataContext.SaveChangesAsync();

            AddBuildingToLandCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddBuildingToLandCommandRequest()
            {
                BuildingTypeId = BuildingsType.DroneFactory,
                UserId = user.Entity.Id,
                LandId = newLand.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {newLand.Entity.Id} is undiscover.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        public async Task AddBuildingToLand_WhenLandNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(10000),
            });
            await mayhemDataContext.SaveChangesAsync();

            AddBuildingToLandCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            AddBuildingToLandCommandRequest request = new()
            {
                BuildingTypeId = BuildingsType.DroneFactory,
                UserId = user.Entity.Id,
                LandId = 212312312,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {request.LandId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task AddBuildingToLand_WhenUserDoesNotHaveResources_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(0),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Field,
            });
            await mayhemDataContext.SaveChangesAsync();

            AddBuildingToLandCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddBuildingToLandCommandRequest()
            {
                BuildingTypeId = BuildingsType.DroneFactory,
                UserId = user.Entity.Id,
                LandId = newLand.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("The user doesn't have enough resource.");
            result.Errors.First().PropertyName.Should().Be("IronOre");
        }

        public async Task AddBuildingToLand_WhenLandDoesNotHaveNpc_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Field,
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        UserId = user.Entity.Id,
                    }
                }
            });
            await mayhemDataContext.SaveChangesAsync();

            AddBuildingToLandCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddBuildingToLandCommandRequest()
            {
                BuildingTypeId = BuildingsType.DroneFactory,
                UserId = user.Entity.Id,
                LandId = newLand.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be("Building DroneFactory cannot be build without Npc/Avatar.");
            result.Errors.First().PropertyName.Should().Be("BuildingId");
        }
    }
}
