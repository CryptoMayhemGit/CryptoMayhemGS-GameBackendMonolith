using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.RemoveTravel;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    internal class RemoveTravelCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task RemoveTravel_WhenNpcNotExist_ThenThrowException_Test()
        {
            int npcId = 1231;
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            RemoveTravelCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new RemoveTravelCommandRequest()
            {
                NpcId = npcId,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npcId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task RemoveTravel_WhenTravelNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, NpcStatusId = Dal.Dto.Enums.Dictionaries.NpcsStatus.None });
            await mayhemDataContext.SaveChangesAsync();

            RemoveTravelCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new RemoveTravelCommandRequest()
            {
                NpcId = npc.Entity.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npc.Entity.Id} doesn't have any travel.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }
    }
}
