using FluentAssertions;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.IntegrationTest.Base;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.MoveNpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class TravelControllerTests : ControllerTestBase<TravelControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [TearDown]
        public async Task TearDown()
        {
            List<Travel> travels = await mayhemDataContext.Travels.ToListAsync();
            foreach (Travel travel in travels)
            {
                mayhemDataContext.Travels.Remove(travel);
            }
            await mayhemDataContext.SaveChangesAsync();

            List<Npc> npcs = await mayhemDataContext.Npcs.ToListAsync();
            foreach (Npc npc in npcs)
            {
                mayhemDataContext.Npcs.Remove(npc);
            }
            await mayhemDataContext.SaveChangesAsync();

            List<UserLand> userLands = await mayhemDataContext.UserLands.ToListAsync();
            foreach (UserLand userLand in userLands)
            {
                mayhemDataContext.UserLands.Remove(userLand);
            }
            await mayhemDataContext.SaveChangesAsync();

            List<Land> lands = await mayhemDataContext.Lands.ToListAsync();
            foreach (Land land in lands)
            {
                mayhemDataContext.Lands.Remove(land);
            }
            await mayhemDataContext.SaveChangesAsync();
        }

        [Test]
        public async Task MoveNpcToLand_WhenNpcNotExist_ThenGetNotFound_Test()
        {
            await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Travel}/Move";

            MoveNpcCommandRequest request = new()
            {
                NpcId = 12341,
                LandToId = landTo.Entity.Id,
            };

            ActionDataResult<MoveNpcCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<MoveNpcCommandRequest, MoveNpcCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task MoveNpcToLand_WhenLandFromNotExist_ThenGetNotFound_Test()
        {
            EntityEntry<Land> landTo = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Travel}/Move";

            MoveNpcCommandRequest request = new()
            {
                NpcId = npc.Entity.Id,
                LandToId = landTo.Entity.Id,
            };

            ActionDataResult<MoveNpcCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<MoveNpcCommandRequest, MoveNpcCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task MoveNpcToLand_WhenLandToNotExist_ThenGetNotFound_Test()
        {
            await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Travel}/Move";

            MoveNpcCommandRequest request = new()
            {
                NpcId = npc.Entity.Id,
                LandToId = 12321,
            };

            ActionDataResult<MoveNpcCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<MoveNpcCommandRequest, MoveNpcCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task MoveNpcToLand_WhenLandToEqualLandFrom_ThenGetBadRequest_Test()
        {
            EntityEntry<Land> landFrom = await mayhemDataContext.Lands.AddAsync(new Land());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = UserId, LandId = landFrom.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Travel}/Move";

            MoveNpcCommandRequest request = new()
            {
                NpcId = npc.Entity.Id,
                LandToId = landFrom.Entity.Id,
            };

            ActionDataResult<MoveNpcCommandResponse> response = await httpClientService.HttpPostAsJsonAsync<MoveNpcCommandRequest, MoveNpcCommandResponse>(endpoint, request, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Errors.First().Message.Should().Contain("LandFrom must be different than LandTo");
        }
    }
}
