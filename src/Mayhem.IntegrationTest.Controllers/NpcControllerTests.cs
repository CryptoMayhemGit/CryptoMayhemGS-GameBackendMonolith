using FluentAssertions;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.IntegrationTest.Base;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.GetAvailableNpcs;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class NpcControllerTests : ControllerTestBase<NpcControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetAvailableNpcs_WhenNpcsNotExists_ThenGetEmpty_Test()
        {
            (_, string token) = await GetNewTokenAsync();

            string endpoint = $"api/{ControllerNames.Npc}/Avaliable";

            ActionDataResult<GetAvailableNpcsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetAvailableNpcsCommandResponse>(endpoint, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Npcs.Should().HaveCount(0);
        }

        [Test]
        public async Task GetAvailableNpcs_WhenNpcsExists_ThenGetThem_Test()
        {
            await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                Building = new Building(),
                UserId = UserId,
            });
            await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                UserId = UserId,
            });
            await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                UserId = UserId,
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Npc}/Avaliable";

            ActionDataResult<GetAvailableNpcsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetAvailableNpcsCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Npcs.Should().HaveCount(2);
        }
    }
}
