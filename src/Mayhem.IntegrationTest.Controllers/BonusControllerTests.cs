using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.IntegrationTest.Base;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.GetMechBonus;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class BonusControllerTests : ControllerTestBase<BonusControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetMechBonusByUserId_WhenUserHasGuildWithFightBoardBuilding_ThenGetIt_Test()
        {
            (int userId, string newToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == userId);

            await mayhemDataContext
                .Guilds
                .AddAsync(new Guild()
                {
                    Users = new List<GameUser>()
                    {
                        user,
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

            string endpoint = $"api/{ControllerNames.Bonus}";

            ActionDataResult<GetMechBonusCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetMechBonusCommandResponse>(endpoint, newToken);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Bonus.Should().Be(2);
        }

        [Test]
        public async Task GetMechBonusByUserId_WhenUserHasNoGuild_ThenGetZero_Test()
        {
            (_, string newToken) = await GetNewTokenAsync();

            string endpoint = $"api/{ControllerNames.Bonus}";

            ActionDataResult<GetMechBonusCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetMechBonusCommandResponse>(endpoint, newToken);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Bonus.Should().Be(0);
        }

        [Test]
        public async Task GetMechBonusByUserId_WhenUserHasGuildWithoutFightBoardBuilding_ThenGetZero_Test()
        {
            (int userId, string newToken) = await GetNewTokenAsync();

            GameUser user = await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Id == userId);

            await mayhemDataContext
               .Guilds
               .AddAsync(new Guild()
               {
                   Users = new List<GameUser>()
                   {
                        user,
                   },
               });

            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Bonus}";

            ActionDataResult<GetMechBonusCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetMechBonusCommandResponse>(endpoint, newToken);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Bonus.Should().Be(0);
        }
    }
}
