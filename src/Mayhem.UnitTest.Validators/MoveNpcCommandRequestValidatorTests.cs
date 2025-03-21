using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Missions;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.MoveNpc;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class MoveNpcCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task AddTravel_WhenNpcNotExist_ThenGetThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            MoveNpcCommandRequest request = new()
            {
                LandToId = landTo.Entity.Id,
                UserId = user.Entity.Id,
                NpcId = 23423,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {request.NpcId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task AddTravel_WhenLandFromNotExist_ThenGetThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });

            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            MoveNpcCommandRequest request = new()
            {
                LandToId = landTo.Entity.Id,
                UserId = user.Entity.Id,
                NpcId = npc.Entity.Id
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npc.Entity.Id} is not in any land.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task AddTravel_WhenLandToNotExist_ThenGetThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { Land = landFrom.Entity, User = user.Entity });
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, Land = landFrom.Entity, });

            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            MoveNpcCommandRequest request = new()
            {
                LandToId = 112234,
                UserId = user.Entity.Id,
                NpcId = npc.Entity.Id
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {request.LandToId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task AddTravel_WhenLandToEqualLandFrom_ThenGetThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, Land = landFrom.Entity, });

            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new MoveNpcCommandRequest()
            {
                LandToId = landFrom.Entity.Id,
                UserId = user.Entity.Id,
                NpcId = npc.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"LandFrom must be different than LandTo.");
        }

        [Test]
        public async Task AddTravel_WhenLandFromAndLandToBelongToDifferentLand_ThenGetThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<LandInstance> landInstance1 = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            EntityEntry<LandInstance> landInstance2 = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land() { LandInstance = landInstance1.Entity });
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land() { LandInstance = landInstance2.Entity });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { Land = landFrom.Entity, User = user.Entity });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { Land = landTo.Entity, User = user.Entity });
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, Land = landFrom.Entity, });

            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new MoveNpcCommandRequest()
            {
                LandToId = landTo.Entity.Id,
                UserId = user.Entity.Id,
                NpcId = npc.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land from and land to must belong to the same land instance.");
            result.Errors.First().PropertyName.Should().Be($"Lands");
        }

        [Test]
        public async Task AddTravel_WhenTravelWithNpcExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, Land = landFrom.Entity, });

            await mayhemDataContext.Travels.AddAsync(new Travel()
            {
                NpcId = npc.Entity.Id,
                LandFromId = landFrom.Entity.Id,
                LandToId = landTo.Entity.Id
            });
            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new MoveNpcCommandRequest()
            {
                LandToId = landTo.Entity.Id,
                UserId = user.Entity.Id,
                NpcId = npc.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npc.Entity.Id} is already on the travel.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task AddTravel_WhenNpcIsOnDiscoveryMission_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, Land = landFrom.Entity, });
            await mayhemDataContext.DiscoveryMissions.AddAsync(new DiscoveryMission()
            {
                NpcId = npc.Entity.Id,
            });

            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new MoveNpcCommandRequest()
            {
                LandToId = landTo.Entity.Id,
                UserId = user.Entity.Id,
                NpcId = npc.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npc.Entity.Id} is busy.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task AddTravel_WhenNpcIsOnExploreMission_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, Land = landFrom.Entity, });
            await mayhemDataContext.ExploreMissions.AddAsync(new ExploreMission()
            {
                NpcId = npc.Entity.Id,
            });

            await mayhemDataContext.SaveChangesAsync();

            MoveNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new MoveNpcCommandRequest()
            {
                LandToId = landTo.Entity.Id,
                UserId = user.Entity.Id,
                NpcId = npc.Entity.Id
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npc.Entity.Id} is busy.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }
    }
}
