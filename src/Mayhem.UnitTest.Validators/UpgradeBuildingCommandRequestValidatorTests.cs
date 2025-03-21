using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.UpgradeBuilding;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class UpgradeBuildingCommandRequestValidatorTests : UnitTestBase
    {
        private IBuildingRepository buildingRepository;
        private IMayhemDataContext mayhemDataContext;
        private ICostsValidationService costsValidationService;
        private IImprovementRepository improvementRepository;
        private IImprovementValidationService improvementValidationService;

        [OneTimeSetUp]
        public void SetUp()
        {
            buildingRepository = GetService<IBuildingRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
            costsValidationService = GetService<ICostsValidationService>();
            improvementRepository = GetService<IImprovementRepository>();
            improvementValidationService = GetService<IImprovementValidationService>();
        }

        [Test]
        public async Task UpgradeBuilding_WhenBuildingNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            });
            await mayhemDataContext.SaveChangesAsync();

            UpgradeBuildingCommandRequestValidator validator = new(mayhemDataContext, buildingRepository, costsValidationService, improvementRepository, improvementValidationService);
            UpgradeBuildingCommandRequest request = new()
            {
                UserId = user.Entity.Id,
                BuildingId = 12332423,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Building with id {request.BuildingId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"BuildingId");
        }

        public async Task UpgradeBuilding_WhenUserDoesntHaveResources_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            });
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandTypeId = LandsType.Mountain,
            });
            await mayhemDataContext.SaveChangesAsync();

            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(newLand.Entity.Id, BuildingsType.Slaughterhouse, user.Entity.Id);
            foreach (UserResource resource in user.Entity.UserResources)
            {
                resource.Value = 0;
            }
            await mayhemDataContext.SaveChangesAsync();

            UpgradeBuildingCommandRequestValidator validator = new(mayhemDataContext, buildingRepository, costsValidationService, improvementRepository, improvementValidationService);
            ValidationResult result = validator.Validate(new UpgradeBuildingCommandRequest()
            {
                UserId = user.Entity.Id,
                BuildingId = building.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"The user doesn't have enough resource.");
            result.Errors.First().PropertyName.Should().Be($"Cereal");
        }
    }
}
