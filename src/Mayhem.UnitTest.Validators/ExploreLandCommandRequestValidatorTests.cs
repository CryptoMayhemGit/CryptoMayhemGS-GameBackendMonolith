using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.ExploreLand;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    internal class ExploreLandCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IExploreMissionRepository exploreMissionRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            exploreMissionRepository = GetService<IExploreMissionRepository>();
        }

        [Test]
        public void ExploreLand_WhenUserIdIsZero_ThenGetMessageError_Test()
        {
            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest());

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"'User Id' must be greater than '0'.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task ExploreLand_WhenLandIdIsZero_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest()
            {
                UserId = user.Entity.Id,
                LandId = 0,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"'Land Id' must be greater than '0'.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task ExploreLand_WhenUserLandNotExist_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest()
            {
                UserId = user.Entity.Id,
                LandId = land.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {land.Entity.Id} doesn't have any hero.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task ExploreLand_WhenUserLandIsAlreadyExplored_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land.Entity.Id, UserId = user.Entity.Id });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.Explored });
            await mayhemDataContext.SaveChangesAsync();

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest()
            {
                UserId = user.Entity.Id,
                LandId = land.Entity.Id,
                NpcId = npc.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {land.Entity.Id} is already explored.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task ExploreLand_WhenUserLandNotHaveAnyNpc_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.Discovered });
            await mayhemDataContext.SaveChangesAsync();

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest()
            {
                UserId = user.Entity.Id,
                LandId = land.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {land.Entity.Id} doesn't have any hero.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task ExploreLand_WhenLandHasWrongType_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land1 = await mayhemDataContext.Lands.AddAsync(new Land() { LandTypeId = LandsType.Forest });
            EntityEntry<Land> land2 = await mayhemDataContext.Lands.AddAsync(new Land() { LandTypeId = LandsType.Water });
            EntityEntry<Npc> npc1 = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land1.Entity.Id, UserId = user.Entity.Id });
            EntityEntry<Npc> npc2 = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land2.Entity.Id, UserId = user.Entity.Id });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land1.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land2.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.Discovered });
            await mayhemDataContext.SaveChangesAsync();

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result1 = validator.Validate(new ExploreLandCommandRequest()
            {
                UserId = user.Entity.Id,
                LandId = land1.Entity.Id,
                NpcId = npc1.Entity.Id,
            });
            ValidationResult result2 = validator.Validate(new ExploreLandCommandRequest()
            {
                UserId = user.Entity.Id,
                LandId = land2.Entity.Id,
                NpcId = npc2.Entity.Id,
            });

            result1.Errors.Should().HaveCount(1);
            result1.Errors.First().ErrorMessage.Should().Be($"Land with id {land1.Entity.Id} has wrong type.");
            result1.Errors.First().PropertyName.Should().Be($"LandId");

            result2.Errors.Should().HaveCount(1);
            result2.Errors.First().ErrorMessage.Should().Be($"Land with id {land2.Entity.Id} has wrong type.");
            result2.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task ExploreLand_WhenLandBelongToOtherUser_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user1 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<GameUser> user2 = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land.Entity.Id, UserId = user1.Entity.Id });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land.Entity.Id, UserId = user1.Entity.Id, Status = LandsStatus.Discovered });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land.Entity.Id, UserId = user2.Entity.Id, Status = LandsStatus.Discovered, Owned = true });
            await mayhemDataContext.SaveChangesAsync();

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest()
            {
                UserId = user1.Entity.Id,
                LandId = land.Entity.Id,
                NpcId = npc.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {land.Entity.Id} belongs to another user.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task ExploreLand_WhenNpcIsBusy_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land.Entity.Id, UserId = user.Entity.Id, NpcStatusId = NpcsStatus.None });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.None });
            await mayhemDataContext.SaveChangesAsync();
            await exploreMissionRepository.ExploreMissionAsync(new ExploreMissionDto()
            {
                NpcId = npc.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land.Entity.Id,
            });

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest()
            {
                NpcId = npc.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npc.Entity.Id} is busy.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task ExploreLand_WhenAnotherNpcIsOnMission_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc1 = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land.Entity.Id, UserId = user.Entity.Id, NpcStatusId = NpcsStatus.None });
            EntityEntry<Npc> npc2 = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land.Entity.Id, UserId = user.Entity.Id, NpcStatusId = NpcsStatus.None });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.None });
            await mayhemDataContext.SaveChangesAsync();
            await exploreMissionRepository.ExploreMissionAsync(new ExploreMissionDto()
            {
                NpcId = npc1.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land.Entity.Id,
            });

            ExploreLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new ExploreLandCommandRequest()
            {
                NpcId = npc2.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Another npc is on a mission on this land.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }
    }
}
