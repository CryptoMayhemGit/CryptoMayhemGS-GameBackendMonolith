using FluentAssertions;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.IntegrationTest.Base;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class NftControllerTests : ControllerTestBase<NftControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;
        private IMayhemConfigurationService mayhemConfigurationService;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            mayhemConfigurationService = GetService<IMayhemConfigurationService>();
        }

        [Test]
        public async Task AddMintedNpc_WhenNpcAdded_ThenGetIt_Test()
        {
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                IsMinted = true,
                Address = "/something",
                Name = "something",
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Nft}/Hero/{newNpc.Entity.Id}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Name.Should().Be(newNpc.Entity.Name);
            response.Result.Image.Should().Be($"{mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.FtpEndpoint}{newNpc.Entity.Address}");
            response.Result.Description.Should().Be(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NftNpcDescription);
        }

        [Test]
        public async Task AddNotMintedNpc_WhenNpcAdded_ThenGetNotFound_Test()
        {
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc()
            {
                IsMinted = false,
                Address = "something",
                Name = "something",
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Nft}/Hero/{newNpc.Entity.Id}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Name.Should().Be("Hero");
            response.Result.Image.Should().Be(mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint);
        }

        [Test]
        public async Task GetNpc_WhenNpcNotExists_ThenGetNotFound_Test()
        {
            string endpoint = $"api/{ControllerNames.Nft}/Hero/{10000}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Name.Should().Be("Hero");
            response.Result.Image.Should().Be(mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint);
        }

        [Test]
        public async Task AddMintedItem_WhenItemAdded_ThenGetIt_Test()
        {
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                IsMinted = true,
                Address = "/something",
                Name = "something",
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Nft}/Item/{newItem.Entity.Id}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Name.Should().Be(newItem.Entity.Name);
            response.Result.Image.Should().Be($"{mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.FtpEndpoint}{newItem.Entity.Address}");
            response.Result.Description.Should().Be(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NftItemDescription);
        }

        [Test]
        public async Task AddNotMintedItem_WhenItemAdded_ThenGetNotFound_Test()
        {
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                IsMinted = false,
                Address = "something",
                Name = "something",
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Nft}/Item/{newItem.Entity.Id}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Name.Should().Be("Item");
            response.Result.Image.Should().Be(mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint);
        }

        [Test]
        public async Task GetItem_WhenItemNotExists_ThenGetNotFound_Test()
        {
            string endpoint = $"api/{ControllerNames.Nft}/Item/{10000}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Name.Should().Be("Item");
            response.Result.Image.Should().Be(mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint);
        }

        [Test]
        public async Task AddMintedLand_WhenLandAdded_ThenGetIt_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                IsMinted = true,
                Address = "/something",
                Name = "something",
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Nft}/Land/{newLand.Entity.Id}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Name.Should().Be(newLand.Entity.Name);
            response.Result.Image.Should().Be($"{mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.FtpEndpoint}{newLand.Entity.Address}");
            response.Result.Description.Should().Be(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NftLandDescription);
        }

        [Test]
        public async Task AddNotMintedLand_WhenLandAdded_ThenGetNotFound_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                IsMinted = false,
                Address = "something",
                Name = "something",
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Nft}/Land/{newLand.Entity.Id}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Name.Should().Be("Land");
            response.Result.Image.Should().Be(mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint);
        }

        [Test]
        public async Task GetLand_WhenLandNotExists_ThenGetNotFound_Test()
        {
            string endpoint = $"api/{ControllerNames.Nft}/Land/{10000}";

            ActionDataResult<NftStandardModel> response = await httpClientService.HttpGetAsJsonAsync<NftStandardModel>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Name.Should().Be("Land");
            response.Result.Image.Should().Be(mayhemConfigurationService.MayhemConfiguration.ServiceDiscoveryConfigruation.NotMintedFtpEndpoint);
        }
    }
}
