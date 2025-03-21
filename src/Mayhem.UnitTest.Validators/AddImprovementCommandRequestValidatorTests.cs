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
using Mayhen.Bl.Commands.AddImprovement;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class AddImprovementCommandRequestValidatorTests : UnitTestBase
    {
        private IImprovementRepository improvementRepository;
        private IMayhemDataContext mayhemDataContext;
        private ICostsValidationService costsValidationService;

        [SetUp]
        public void SetUp()
        {
            improvementRepository = GetService<IImprovementRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
            costsValidationService = GetService<ICostsValidationService>();
        }

        [Test]
        public async Task AddExistingImprovement_WhenImprovementExists_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            })).Entity.Id;

            long landId = (await mayhemDataContext.Lands.AddAsync(new Land())).Entity.Id;

            await mayhemDataContext.SaveChangesAsync();

            await improvementRepository.AddImprovementAsync(new ImprovementDto()
            {
                ImprovementTypeId = ImprovementsType.BasicMiningCombine,
                LandId = landId,
            }, userId);

            AddImprovementCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            AddImprovementCommandRequest request = new()
            {
                LandId = landId,
                UserId = userId,
                ImprovementTypeId = ImprovementsType.BasicMiningCombine,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Improvement with landId {landId} and ImprovementsTypeId {request.ImprovementTypeId} already exists.");
            result.Errors.First().PropertyName.Should().Be($"ImprovementId");
        }

        [Test]
        public async Task AddImprovementAsync_WhenLandNotExists_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser()
            {
                UserResources = ResourceHelper.GetBasicUserResourcesWithValue(),
            })).Entity.Id;

            await mayhemDataContext.SaveChangesAsync();

            AddImprovementCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            AddImprovementCommandRequest request = new()
            {
                ImprovementTypeId = ImprovementsType.BasicMiningCombine,
                LandId = 23432,
                UserId = userId,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {request.LandId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }
    }
}
