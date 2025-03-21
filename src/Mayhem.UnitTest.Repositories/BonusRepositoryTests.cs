using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class BonusRepositoryTests : UnitTestBase
    {
        private IBonusRepository bonusLogRepository;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            bonusLogRepository = GetService<IBonusRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetMechBonusByUserId_WhenUserHasGuildWithFightBoardBuilding_ThenGetIt_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext
                .GameUsers
                .AddAsync(new GameUser());

            await mayhemDataContext
                .Guilds
                .AddAsync(new Guild()
                {
                    Users = new List<GameUser>()
                    {
                        user.Entity,
                    },
                    GuildBuildings = new List<GuildBuilding>()
                    {
                        new GuildBuilding()
                        {
                            GuildBuildingTypeId = GuildBuildingsType.FightBoard,
                            GuildBuildingBonuses = new List<GuildBuildingBonus>()
                            {
                                new GuildBuildingBonus()
                                {
                                    GuildBuildingBonusTypeId = GuildBuildingBonusesType.MechAttack,
                                    Bonus = 2,
                                }
                            }
                        }
                    }
                });

            await mayhemDataContext.SaveChangesAsync();

            double bonus = await bonusLogRepository.GetMechBonusByUserIdAsync(user.Entity.Id);

            bonus.Should().Be(2);
        }

        [Test]
        public async Task GetMechBonusByUserId_WhenUserHasNoGuild_ThenGetZero_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext
                .GameUsers
                .AddAsync(new GameUser());

            await mayhemDataContext.SaveChangesAsync();

            double bonus = await bonusLogRepository.GetMechBonusByUserIdAsync(user.Entity.Id);

            bonus.Should().Be(0);
        }

        [Test]
        public async Task GetMechBonusByUserId_WhenUserHasGuildWithoutFightBoardBuilding_ThenGetZero_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext
                .GameUsers
                .AddAsync(new GameUser());

            await mayhemDataContext
               .Guilds
               .AddAsync(new Guild()
               {
                   Users = new List<GameUser>()
                   {
                        user.Entity,
                   },
               });

            await mayhemDataContext.SaveChangesAsync();

            double bonus = await bonusLogRepository.GetMechBonusByUserIdAsync(user.Entity.Id);

            bonus.Should().Be(0);
        }
    }
}
