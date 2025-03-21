using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.DiscoverLand;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class DiscoverLandCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IDiscoveryMissionRepository discoveryMissionRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            discoveryMissionRepository = GetService<IDiscoveryMissionRepository>();
        }

        [Test]
        public void DiscoverLand_WhenUserIdIsZero_ThenGetMessageError_Test()
        {
            DiscoverLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DiscoverLandCommandRequest());

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"'User Id' must be greater than '0'.");
            result.Errors.First().PropertyName.Should().Be($"UserId");
        }

        [Test]
        public async Task DiscoverLand_WhenLandIdIsZero_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.SaveChangesAsync();

            DiscoverLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DiscoverLandCommandRequest()
            {
                UserId = user.Entity.Id,
                LandId = 0,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"'Land Id' must be greater than '0'.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public void DiscoverLand_WhenLandDoesNotExist_ThenGetMessageError_Test()
        {
            DiscoverLandCommandRequestValidator validator = new(mayhemDataContext);
            DiscoverLandCommandRequest request = new()
            {
                LandId = 231,
                UserId = 4,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {request.LandId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task DiscoverLand_WhenLandIsAlreadyExploredForUser_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<UserLand> userLand = await mayhemDataContext.UserLands.AddAsync(new UserLand()
            {
                Status = LandsStatus.Explored,
                User = user.Entity,
                Land = new Land(),
            });

            await mayhemDataContext.SaveChangesAsync();

            DiscoverLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DiscoverLandCommandRequest()
            {
                LandId = userLand.Entity.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {userLand.Entity.Id} is already discovered.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task DiscoverLand_WhenLandDoesNotHaveAnyHero_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                UserLands = new List<UserLand>()
                {
                    new UserLand()
                    {
                        User = user.Entity,
                        Status = LandsStatus.None
                    }
                }
            });

            await mayhemDataContext.SaveChangesAsync();

            DiscoverLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DiscoverLandCommandRequest()
            {
                LandId = land.Entity.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Land with id {land.Entity.Id} doesn't have any hero.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }

        [Test]
        public async Task DiscoverLand_WhenNpcIsOnMission_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land1 = await mayhemDataContext.Lands.AddAsync(new Land() { PositionX = 1, PositionY = 1 });
            EntityEntry<Land> land2 = await mayhemDataContext.Lands.AddAsync(new Land() { PositionX = 1, PositionY = 0 });
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land1.Entity.Id, UserId = user.Entity.Id });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land1.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.None });
            await mayhemDataContext.SaveChangesAsync();
            await discoveryMissionRepository.DiscoverMissionAsync(new DiscoveryMissionDto()
            {
                NpcId = npc.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land2.Entity.Id,
            });

            DiscoverLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DiscoverLandCommandRequest()
            {
                NpcId = npc.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land2.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {npc.Entity.Id} is busy.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task ExploreLand_WhenAnotherNpcIsOnMission_ThenGetMessageError_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Land> land1 = await mayhemDataContext.Lands.AddAsync(new Land() { PositionX = 1, PositionY = 1 });
            EntityEntry<Land> land2 = await mayhemDataContext.Lands.AddAsync(new Land() { PositionX = 1, PositionY = 0 });
            EntityEntry<Npc> npc1 = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land1.Entity.Id, UserId = user.Entity.Id, NpcStatusId = NpcsStatus.None });
            EntityEntry<Npc> npc2 = await mayhemDataContext.Npcs.AddAsync(new Npc() { LandId = land1.Entity.Id, UserId = user.Entity.Id, NpcStatusId = NpcsStatus.None });
            await mayhemDataContext.UserLands.AddAsync(new UserLand() { LandId = land1.Entity.Id, UserId = user.Entity.Id, Status = LandsStatus.None });
            await mayhemDataContext.SaveChangesAsync();
            await discoveryMissionRepository.DiscoverMissionAsync(new DiscoveryMissionDto()
            {
                NpcId = npc1.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land2.Entity.Id,
            });

            DiscoverLandCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new DiscoverLandCommandRequest()
            {
                NpcId = npc2.Entity.Id,
                UserId = user.Entity.Id,
                LandId = land2.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Another npc is on a mission on this land.");
            result.Errors.First().PropertyName.Should().Be($"LandId");
        }
    }
}
