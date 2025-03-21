using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.AddGuildImprovement;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class AddGuildImprovementCommandRequestValidatorTests : UnitTestBase
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
        public async Task AddExistingGuildImprovement_WhenGuildImprovementExists_ThenThrowException_Test()
        {
            int userId = (await mayhemDataContext.GameUsers.AddAsync(new GameUser())).Entity.Id;
            int guildId = (await mayhemDataContext.Guilds.AddAsync(new Guild()
            {
                GuildResources = ResourceHelper.GetBasicGuildResourcesWithValue(),
                OwnerId = userId,
            })).Entity.Id;
            await mayhemDataContext.SaveChangesAsync();

            GuildImprovementsType improvement = GuildImprovementsType.LargerWheels;

            await improvementRepository.AddGuildImprovementAsync(new GuildImprovementDto()
            {
                GuildImprovementTypeId = improvement,
                GuildId = guildId,
            });

            AddGuildImprovementCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddGuildImprovementCommandRequest()
            {
                GuildImprovementTypeId = improvement,
                GuildId = guildId,
                UserId = userId,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Improvement with guildId {guildId} and ImprovementsTypeId {improvement} already exists.");
            result.Errors.First().PropertyName.Should().Be($"GuildImprovement");
        }

        [Test]
        public void AddGuildImprovementAsync_WhenGuildNotExists_ThenThrowException_Test()
        {
            int guildId = 223234;

            AddGuildImprovementCommandRequestValidator validator = new(costsValidationService, mayhemDataContext);
            ValidationResult result = validator.Validate(new AddGuildImprovementCommandRequest()
            {
                GuildImprovementTypeId = GuildImprovementsType.ImprovedTransmission,
                GuildId = guildId,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Guild doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"Guild");
        }
    }
}
