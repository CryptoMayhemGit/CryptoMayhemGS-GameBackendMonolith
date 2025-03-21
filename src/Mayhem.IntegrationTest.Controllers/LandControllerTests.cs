using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.IntegrationTest.Base;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.CheckPurchaseLand;
using Mayhen.Bl.Commands.GetInstance;
using Mayhen.Bl.Commands.GetLandDetails;
using Mayhen.Bl.Commands.GetLandStatus;
using Mayhen.Bl.Commands.GetUserLands;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class LandControllerTests : ControllerTestBase<LandControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetLandDetails_WhenLandIsDiscovered_ThenGetIt_Test()
        {
            EntityEntry<UserLand> newLand = await mayhemDataContext.UserLands.AddAsync(new UserLand()
            {
                Land = new Land()
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/{newLand.Entity.Id}/Details";

            ActionDataResult<GetLandDetailsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetLandDetailsCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.UserLand.Should().NotBeNull();
        }

        [Test]
        public async Task GetLandDetails_WhenLandIsNotDiscovered_ThenGetIt_Test()
        {
            EntityEntry<UserLand> newUserLand = await mayhemDataContext.UserLands.AddAsync(new UserLand()
            {
                UserId = UserId,
                Land = new Land(),
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/{newUserLand.Entity.Id}/Details";

            ActionDataResult<GetLandDetailsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetLandDetailsCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.UserLand.Should().NotBeNull();
        }

        [Test]
        public async Task GetLandDetails_WhenLandNotExist_ThenGetNull_Test()
        {
            string endpoint = $"api/{ControllerNames.Land}/{1023}/Details";

            ActionDataResult<GetLandDetailsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetLandDetailsCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.Result.Should().BeNull();
        }


        [Test]
        public async Task GetLandStatus_WhenLandExist_ThenGetStatuses_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.Jobs.AddRangeAsync(
                new Job()
                {
                    LandId = newLand.Entity.Id
                },
                new Job()
                {
                    LandId = newLand.Entity.Id
                }
            );

            await mayhemDataContext.Travels.AddRangeAsync(
                new Travel()
                {
                    LandFromId = newLand.Entity.Id
                },
                new Travel()
                {
                    LandToId = newLand.Entity.Id
                }
            );

            await mayhemDataContext.Buildings.AddRangeAsync(
                new Building()
                {
                    LandId = newLand.Entity.Id
                },
                new Building()
                {
                    LandId = newLand.Entity.Id
                }
            );

            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/{newLand.Entity.Id}/Status";

            ActionDataResult<GetLandStatusCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetLandStatusCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Operations.Should().HaveCount(6);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.Building).Should().HaveCount(2);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.Job).Should().HaveCount(2);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.TravelFrom).Should().HaveCount(1);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.TravelTo).Should().HaveCount(1);
        }

        [Test]
        public async Task GetLandStatus_WhenLandExist_ThenGetSomeStatuses_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.Jobs.AddRangeAsync(
                new Job()
                {
                    LandId = newLand.Entity.Id
                }
            );

            await mayhemDataContext.Buildings.AddRangeAsync(
                new Building()
                {
                    LandId = newLand.Entity.Id
                }
            );

            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/{newLand.Entity.Id}/Status";

            ActionDataResult<GetLandStatusCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetLandStatusCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Operations.Should().HaveCount(2);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.Building).Should().HaveCount(1);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.Job).Should().HaveCount(1);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.TravelFrom).Should().HaveCount(0);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.TravelTo).Should().HaveCount(0);
        }

        [Test]
        public async Task GetLandStatus_WhenLandNotExist_ThenGetEmpty_Test()
        {
            string endpoint = $"api/{ControllerNames.Land}/{1234}/Status";

            ActionDataResult<GetLandStatusCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetLandStatusCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Operations.Should().HaveCount(0);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.Building).Should().HaveCount(0);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.Job).Should().HaveCount(0);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.TravelFrom).Should().HaveCount(0);
            response.Result.Operations.Where(x => x.OperationType == LandOperationsType.TravelTo).Should().HaveCount(0);
        }

        [Test]
        public async Task GetLandsByInstanceId_WhenLandsExists_ThenGetThem_Test()
        {
            const int expectedLandsCount = 5;

            EntityEntry<LandInstance> newLandInstance = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            for (int i = 0; i < expectedLandsCount; i++)
            {
                await mayhemDataContext.Lands.AddAsync(new Land() { LandInstanceId = newLandInstance.Entity.Id });
            }
            await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.Lands.AddAsync(new Land());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/{newLandInstance.Entity.Id}/Instance";

            ActionDataResult<GetLandInstanceCommandResponse> response =
                await httpClientService.HttpGetAsJsonAsync<GetLandInstanceCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Lands.Should().HaveCount(expectedLandsCount);
        }

        [Test]
        public async Task GetUserLands_WhenLandsExists_ThenGetThem_Test()
        {
            const int expectedLandsCount = 7;

            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<LandInstance> newLandInstance = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            for (int i = 0; i < expectedLandsCount; i++)
            {
                await mayhemDataContext.Lands.AddAsync(new Land() { LandInstanceId = newLandInstance.Entity.Id });
            }
            await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/User";

            ActionDataResult<GetUserLandsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetUserLandsCommandResponse>(endpoint, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.UserLands.All(x => x != null).Should().BeTrue();
        }

        [Test]
        public async Task CheckPurchaseLand_WhenLandIsNeutralAndExplored_ThenGetTrue_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<LandInstance> newLandInstance = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandInstanceId = newLandInstance.Entity.Id,
            });
            await mayhemDataContext.UserLands.AddAsync(new UserLand()
            {
                UserId = userId,
                LandId = land.Entity.Id,
                Status = LandsStatus.Explored,
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/Purchase/Check/{land.Entity.Id}";

            ActionDataResult<CheckPurchaseLandCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<CheckPurchaseLandCommandResponse>(endpoint, token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Result.Should().BeTrue();
        }

        [Test]
        public async Task CheckPurchaseLand_WhenLandBelongToOtherUserAndIsOnSale_ThenGetTrue_Test()
        {
            (int user1Id, _) = await GetNewTokenAsync();
            (_, string token2) = await GetNewTokenAsync();
            EntityEntry<LandInstance> newLandInstance = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            EntityEntry<Land> land = await mayhemDataContext.Lands.AddAsync(new Land()
            {
                LandInstanceId = newLandInstance.Entity.Id,
            });
            await mayhemDataContext.UserLands.AddAsync(new UserLand()
            {
                UserId = user1Id,
                LandId = land.Entity.Id,
                Status = LandsStatus.Explored,
                Owned = true,
                OnSale = true
            });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Land}/Purchase/Check/{land.Entity.Id}";

            ActionDataResult<CheckPurchaseLandCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<CheckPurchaseLandCommandResponse>(endpoint, token2);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Result.Should().NotBeNull();
            response.Result.Result.Should().BeTrue();
        }
    }
}
