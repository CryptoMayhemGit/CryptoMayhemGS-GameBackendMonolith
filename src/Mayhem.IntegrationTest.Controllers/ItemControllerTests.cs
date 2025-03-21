using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.IntegrationTest.Base;
using Mayhem.Test.Common;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.AssignItemToNpc;
using Mayhen.Bl.Commands.GetAvailableItems;
using Mayhen.Bl.Commands.GetUnavailableItems;
using Mayhen.Bl.Commands.ReleaseItemFromNpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Controllers
{
    public class ItemControllerTests : ControllerTestBase<ItemControllerTests>
    {
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void Setup()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test, Order(1)]
        public async Task GetAvailableItem_WhenItemsExists_ThenGetThem_Test()
        {
            await mayhemDataContext.Items.AddAsync(new Item() { UserId = UserId, IsMinted = true });
            await mayhemDataContext.Items.AddAsync(new Item() { UserId = UserId, IsMinted = true });
            await mayhemDataContext.Items.AddAsync(new Item() { UserId = UserId, IsMinted = true });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Available";

            ActionDataResult<GetAvailableItemsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetAvailableItemsCommandResponse>(endpoint, Token);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Items.Should().HaveCount(3);
        }

        [Test]
        public async Task GetAvailableItem_WhenItemsNotExists_ThenGetEmpty_Test()
        {
            (_, string newToken) = await GetNewTokenAsync();
            string endpoint = $"api/{ControllerNames.Item}/Available";

            ActionDataResult<GetAvailableItemsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetAvailableItemsCommandResponse>(endpoint, newToken);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Items.Should().HaveCount(0);
        }

        [Test]
        public async Task GetUnavailableItem_WhenItemsExists_ThenGetThem_Test()
        {
            (int newUserId, string newToken) = await GetNewTokenAsync();
            await mayhemDataContext.Items.AddAsync(new Item() { UserId = newUserId, IsUsed = true, });
            await mayhemDataContext.Items.AddAsync(new Item() { UserId = newUserId, IsUsed = true, });
            await mayhemDataContext.Items.AddAsync(new Item() { UserId = newUserId, IsUsed = true, });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Unavailable";

            ActionDataResult<GetUnavailableItemsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetUnavailableItemsCommandResponse>(endpoint, newToken);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Items.Should().HaveCount(3);
        }

        [Test]
        public async Task GetUnavailableItem_WhenItemsNotExists_ThenGetEmpty_Test()
        {
            (_, string newToken) = await GetNewTokenAsync();
            string endpoint = $"api/{ControllerNames.Item}/Unavailable";

            ActionDataResult<GetAvailableItemsCommandResponse> response = await httpClientService.HttpGetAsJsonAsync<GetAvailableItemsCommandResponse>(endpoint, newToken);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Result.Items.Should().HaveCount(0);
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemAndNpcExists_ThenGetIt_Test()
        {
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item() { UserId = UserId, IsMinted = true });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = UserId });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc.Entity.Id,
            };

            ActionDataResult<AssignItemToNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, Token);

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            response.Result.Status.Should().BeTrue();
            item.IsUsed.Should().BeTrue();
            item.Npc.Should().NotBeNull();
            npc.Item.Should().NotBeNull();
            npc.ItemId.Should().Be(item.Id);
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemIsNotMinted_ThenGetNotFound_Test()
        {
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item() { UserId = UserId });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = UserId });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc.Entity.Id,
            };

            ActionDataResult<AssignItemToNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
            response.Errors.First().Message.Should().Be($"Item with id {newItem.Entity.Id} is not minted.");
            response.Errors.First().FieldName.Should().Be($"ItemId");
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemNotExist_ThenGetBadRequest_Test()
        {
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = UserId });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = 2312,
                NpcId = npc.Entity.Id,
            };

            ActionDataResult<AssignItemToNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
        }

        [Test]
        public async Task AssignItemToNpc_WhenNpcNotExist_ThenGetBadRequest_Test()
        {
            EntityEntry<Item> item = await mayhemDataContext.Items.AddAsync(new Item());
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = item.Entity.Id,
                NpcId = 1234,
            };

            ActionDataResult<AssignItemToNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
        }

        [Test]
        public async Task AssignItemToNpc_WhenNpcAndItemNotExists_ThenGetBadRequest_Test()
        {
            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = 1423,
                NpcId = 1234,
            };

            ActionDataResult<AssignItemToNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
        }

        [Test]
        public async Task ReleaseItemFromNpc_WhenItemExist_ThenGetIt_Test()
        {
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item() { UserId = UserId, IsMinted = true });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = UserId });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc.Entity.Id,
            };

            await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, Token);

            ReleaseItemFromNpcCommandRequest releaseItemFromNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
            };

            string releaseEndpoint = $"api/{ControllerNames.Item}/Release";

            ActionDataResult<ReleaseItemFromNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<ReleaseItemFromNpcCommandRequest, ReleaseItemFromNpcCommandResponse>(releaseEndpoint, releaseItemFromNpcCommandRequest, Token);


            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            response.Result.Status.Should().BeTrue();
            item.IsUsed.Should().BeFalse();
            item.Npc.Should().BeNull();
            npc.Item.Should().BeNull();
            npc.ItemId.Should().BeNull();
        }

        [Test]
        public async Task ReleaseItemFromNpc_WhenItemNotExist_ThenGetBadRequest_Test()
        {
            string releaseEndpoint = $"api/{ControllerNames.Item}/Release";

            ReleaseItemFromNpcCommandRequest releaseItemFromNpcCommandRequest = new()
            {
                ItemId = 1234,
            };

            ActionDataResult<ReleaseItemFromNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<ReleaseItemFromNpcCommandRequest, ReleaseItemFromNpcCommandResponse>(releaseEndpoint, releaseItemFromNpcCommandRequest, Token);

            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Result.Should().BeNull();
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemAndNpcExists_ThenIncreaseAttributesAndGetTrue_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                UserId = userId,
                IsMinted = true,
                ItemTypeId = ItemsType.ContainerTrailer,
                ItemBonuses = new List<ItemBonus>()
            {
                new ItemBonus() { ItemBonusTypeId = ItemBonusesType.MoveSpeed, Bonus = 4 }
            }
            });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = userId, NpcTypeId = NpcsType.Lumberjack, Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack) });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc.Entity.Id,
            };

            await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, token);

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);

            item.Npc.Should().NotBeNull();
            npc.Item.Should().NotBeNull();
            npc.Item.IsUsed.Should().BeTrue();
            npc.ItemId.Should().Be(newItem.Entity.Id);
            npc.Attributes.Should().HaveCount(16);
            npc.Attributes.Where(x => x.AttributeTypeId == AttributesType.MoveSpeed).SingleOrDefault().Value.Should().Be(1.04);
        }

        [Test]
        public async Task ReleaseItemFromNpc_WhenItemExist_ThenDecreaseAttributesAndGetTrue_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                UserId = userId,
                IsMinted = true,
                ItemTypeId = ItemsType.LaserRifle,
                ItemBonuses = new List<ItemBonus>()
            {
                new ItemBonus() { ItemBonusTypeId = ItemBonusesType.Attack, Bonus = 3 }
            }
            });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = userId, NpcTypeId = NpcsType.Farmer, Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Farmer) });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc.Entity.Id,
            };

            await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, token);
            double attributeValueBefore = (await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id)).Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).Select(x => x.Value).SingleOrDefault();

            ReleaseItemFromNpcCommandRequest releaseItemFromNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
            };

            string releaseEndpoint = $"api/{ControllerNames.Item}/Release";

            ActionDataResult<ReleaseItemFromNpcCommandResponse> response =
                await httpClientService.HttpPutAsJsonAsync<ReleaseItemFromNpcCommandRequest, ReleaseItemFromNpcCommandResponse>(releaseEndpoint, releaseItemFromNpcCommandRequest, token);

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);


            item.Npc.Should().BeNull();
            item.IsUsed.Should().BeFalse();
            npc.Attributes.Should().HaveCount(16);
            attributeValueBefore.Should().Be(1.03);
            npc.Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(1);
        }

        [Test]
        public async Task AssingAndReleaseItemFromNpcManyTime_WhenItemExist_ThenAttributesValueShouldBeCorrect_Test()
        {
            (int userId, string token) = await GetNewTokenAsync();
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                UserId = userId,
                IsMinted = true,
                ItemTypeId = ItemsType.Harvester,
                ItemBonuses = new List<ItemBonus>()
            {
                new ItemBonus() { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 4 }
            }
            });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = userId, NpcTypeId = NpcsType.Miner, Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Miner) });
            await mayhemDataContext.SaveChangesAsync();

            string endpoint = $"api/{ControllerNames.Item}/Assign";
            string releaseEndpoint = $"api/{ControllerNames.Item}/Release";

            double lightWoodBonusBeforeRelease = 0;
            double heavyWoodBonusBeforeRelease = 0;

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc.Entity.Id,
            };

            ReleaseItemFromNpcCommandRequest releaseItemFromNpcCommandRequest = new()
            {
                ItemId = newItem.Entity.Id,
            };

            for (int i = 0; i < 20; i++)
            {
                await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(endpoint, assignItemToNpcCommandRequest, token);
                if (i == 0)
                {
                    lightWoodBonusBeforeRelease = (await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id)).Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).Select(x => x.Value).SingleOrDefault();
                    heavyWoodBonusBeforeRelease = (await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id)).Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).Select(x => x.Value).SingleOrDefault();
                }
                await httpClientService.HttpPutAsJsonAsync<ReleaseItemFromNpcCommandRequest, ReleaseItemFromNpcCommandResponse>(releaseEndpoint, releaseItemFromNpcCommandRequest, token);
            }

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);


            item.Npc.Should().BeNull();
            npc.Attributes.Should().HaveCount(16);
            lightWoodBonusBeforeRelease.Should().Be(0.78);
            heavyWoodBonusBeforeRelease.Should().Be(0.416);
            npc.Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.75);
            npc.Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.4);
        }

        [Test]
        public async Task ReleaseItemAndCheckStateAfterAndBefor_Test()
        {
            (int newUserId, string newToken) = await GetNewTokenAsync();
            EntityEntry<Item> item1 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = newUserId, IsMinted = true });
            EntityEntry<Item> item2 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = newUserId, IsMinted = true });
            EntityEntry<Item> item3 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = newUserId, IsMinted = true });
            EntityEntry<Npc> npc1 = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = newUserId });
            EntityEntry<Npc> npc2 = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = newUserId });
            await mayhemDataContext.SaveChangesAsync();

            string assignEndpoint = $"api/{ControllerNames.Item}/Assign";
            string releaseEndpoint = $"api/{ControllerNames.Item}/Release";
            string availableEndpoint = $"api/{ControllerNames.Item}/Available";
            string unavailableEndpoint = $"api/{ControllerNames.Item}/Unavailable";


            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest1 = new()
            {
                ItemId = item1.Entity.Id,
                NpcId = npc1.Entity.Id,
            };

            AssignItemToNpcCommandRequest assignItemToNpcCommandRequest2 = new()
            {
                ItemId = item2.Entity.Id,
                NpcId = npc2.Entity.Id,
            };

            ActionDataResult<GetAvailableItemsCommandResponse> availableResponse = await httpClientService.HttpGetAsJsonAsync<GetAvailableItemsCommandResponse>(availableEndpoint, newToken);
            ActionDataResult<GetUnavailableItemsCommandResponse> unavailableResponse = await httpClientService.HttpGetAsJsonAsync<GetUnavailableItemsCommandResponse>(unavailableEndpoint, newToken);

            IEnumerable<ItemDto> availableItems = availableResponse.Result.Items;
            IEnumerable<ItemDto> unavailableItems = unavailableResponse.Result.Items;

            availableItems.Should().HaveCount(3);
            unavailableItems.Should().HaveCount(0);

            ActionDataResult<AssignItemToNpcCommandResponse> assignResponse1 =
                await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(assignEndpoint, assignItemToNpcCommandRequest1, newToken);

            ActionDataResult<AssignItemToNpcCommandResponse> assignResponse2 =
               await httpClientService.HttpPutAsJsonAsync<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>(assignEndpoint, assignItemToNpcCommandRequest2, newToken);

            Item itemDb1 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item1.Entity.Id);
            Item itemDb2 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item2.Entity.Id);

            Npc npcDb1 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc1.Entity.Id);
            Npc npcDb2 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc2.Entity.Id);

            availableResponse = await httpClientService.HttpGetAsJsonAsync<GetAvailableItemsCommandResponse>(availableEndpoint, newToken);
            unavailableResponse = await httpClientService.HttpGetAsJsonAsync<GetUnavailableItemsCommandResponse>(unavailableEndpoint, newToken);

            availableItems = availableResponse.Result.Items;
            unavailableItems = unavailableResponse.Result.Items;

            itemDb1.Npc.Should().NotBeNull();
            itemDb1.IsUsed.Should().BeTrue();
            itemDb1.Npc.ItemId.Should().Be(item1.Entity.Id);

            itemDb2.Npc.Should().NotBeNull();
            itemDb2.IsUsed.Should().BeTrue();
            itemDb2.Npc.ItemId.Should().Be(item2.Entity.Id);

            npcDb1.Item.Should().NotBeNull();
            npcDb1.ItemId.Should().Be(item1.Entity.Id);

            npcDb2.Item.Should().NotBeNull();
            npcDb2.ItemId.Should().Be(item2.Entity.Id);

            availableItems.Should().HaveCount(1);
            unavailableItems.Should().HaveCount(2);

            ReleaseItemFromNpcCommandRequest releaseItemFromNpcCommandRequest1 = new()
            {
                ItemId = item1.Entity.Id,
            };

            ReleaseItemFromNpcCommandRequest releaseItemFromNpcCommandRequest2 = new()
            {
                ItemId = item2.Entity.Id,
            };

            ActionDataResult<ReleaseItemFromNpcCommandResponse> releaseResponse1 =
                await httpClientService.HttpPutAsJsonAsync<ReleaseItemFromNpcCommandRequest, ReleaseItemFromNpcCommandResponse>(releaseEndpoint, releaseItemFromNpcCommandRequest1, newToken);

            ActionDataResult<ReleaseItemFromNpcCommandResponse> releaseResponse2 =
               await httpClientService.HttpPutAsJsonAsync<ReleaseItemFromNpcCommandRequest, ReleaseItemFromNpcCommandResponse>(releaseEndpoint, releaseItemFromNpcCommandRequest2, newToken);

            itemDb1 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item1.Entity.Id);
            itemDb2 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item2.Entity.Id);

            npcDb1 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc1.Entity.Id);
            npcDb2 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc2.Entity.Id);

            availableResponse = await httpClientService.HttpGetAsJsonAsync<GetAvailableItemsCommandResponse>(availableEndpoint, newToken);
            unavailableResponse = await httpClientService.HttpGetAsJsonAsync<GetUnavailableItemsCommandResponse>(unavailableEndpoint, newToken);

            availableItems = availableResponse.Result.Items;
            unavailableItems = unavailableResponse.Result.Items;

            itemDb1.Npc.Should().BeNull();
            itemDb1.IsUsed.Should().BeFalse();

            itemDb2.Npc.Should().BeNull();
            itemDb2.IsUsed.Should().BeFalse();

            npcDb1.Item.Should().BeNull();
            npcDb1.ItemId.Should().BeNull();

            npcDb2.Item.Should().BeNull();
            npcDb2.ItemId.Should().BeNull();

            availableItems.Should().HaveCount(3);
            unavailableItems.Should().HaveCount(0);
        }
    }
}
