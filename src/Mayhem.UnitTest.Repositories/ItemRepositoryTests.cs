using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Test.Common;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class ItemRepositoryTests : UnitTestBase
    {
        private IItemRepository itemRepository;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            itemRepository = GetService<IItemRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetItemById_WhenItemExists_ThenGetIt_Test()
        {
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item());
            await mayhemDataContext.SaveChangesAsync();

            ItemDto item = await itemRepository.GetItemByNftIdAsync(newItem.Entity.Id);

            item.Should().NotBeNull();
        }

        [Test]
        public async Task GetItemById_WhenItemNotExists_ThenGetNull_Test()
        {
            ItemDto item = await itemRepository.GetItemByNftIdAsync(1232);

            item.Should().BeNull();
        }

        [Test]
        public async Task GetAvailableItemsByUserId_WhenItemsExists_ThenGetThem_Test()
        {
            EntityEntry<GameUser> gameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.Items.AddRangeAsync(
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                },
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                },
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                    IsUsed = true,
                });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<ItemDto> items = await itemRepository.GetAvailableItemsByUserIdAsync(gameUser.Entity.Id);

            items.Should().NotBeNull();
            items.Should().HaveCount(2);
        }

        [Test]
        public async Task GetAvailableItemsByUserId_WhenItemsNotExists_ThenGetEmpty_Test()
        {
            EntityEntry<GameUser> gameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.Items.AddRangeAsync(
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                    IsUsed = true,
                },
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                    IsUsed = true,
                });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<ItemDto> items = await itemRepository.GetAvailableItemsByUserIdAsync(gameUser.Entity.Id);

            items.Should().NotBeNull();
            items.Should().HaveCount(0);
        }

        [Test]
        public async Task GetUnavailableItemsByUserId_WhenItemsExists_ThenGetThem_Test()
        {
            EntityEntry<GameUser> gameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.Items.AddRangeAsync(
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                    IsUsed = true,
                },
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                    IsUsed = true,
                },
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<ItemDto> items = await itemRepository.GetUnavailableItemsByUserIdAsync(gameUser.Entity.Id);

            items.Should().NotBeNull();
            items.Should().HaveCount(2);
        }

        [Test]
        public async Task GetUnavailableItemsByUserId_WhenItemsNotExists_ThenGetEmpty_Test()
        {
            EntityEntry<GameUser> gameUser = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            await mayhemDataContext.Items.AddRangeAsync(
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                },
                new Item()
                {
                    UserId = gameUser.Entity.Id,
                });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<ItemDto> items = await itemRepository.GetUnavailableItemsByUserIdAsync(gameUser.Entity.Id);

            items.Should().NotBeNull();
            items.Should().HaveCount(0);
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemAndNpcExists_ThenGetTrue_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            bool status = await itemRepository.AssignItemToNpcAsync(newNpc.Entity.Id, newItem.Entity.Id, user.Entity.Id);

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);

            status.Should().BeTrue();
            item.Npc.Should().NotBeNull();
            npc.Item.Should().NotBeNull();
            npc.Item.IsUsed.Should().BeTrue();
            npc.ItemId.Should().Be(newItem.Entity.Id);
        }

        [Test]
        public async Task ReleaseItemFromNpc_WhenItemExist_ThenGetTrue_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            await itemRepository.AssignItemToNpcAsync(newNpc.Entity.Id, newItem.Entity.Id, user.Entity.Id);
            bool status = await itemRepository.ReleaseItemFromNpcAsync(newItem.Entity.Id, user.Entity.Id);

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);


            status.Should().BeTrue();
            item.Npc.Should().BeNull();
            item.IsUsed.Should().BeFalse();
            npc.Item.Should().BeNull();
            npc.ItemId.Should().BeNull();
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemAndNpcExists_ThenIncreaseAttributesAndGetTrue_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                UserId = user.Entity.Id,
                IsMinted = true,
                ItemTypeId = ItemsType.ContainerTrailer,
                ItemBonuses = new List<ItemBonus>()
                {
                    new ItemBonus() { ItemBonusTypeId = ItemBonusesType.MoveSpeed, Bonus = 4 }
                }
            });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, NpcTypeId = NpcsType.Lumberjack, Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Lumberjack) });
            await mayhemDataContext.SaveChangesAsync();

            bool status = await itemRepository.AssignItemToNpcAsync(newNpc.Entity.Id, newItem.Entity.Id, user.Entity.Id);

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);

            status.Should().BeTrue();
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
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                UserId = user.Entity.Id,
                IsMinted = true,
                ItemTypeId = ItemsType.LaserRifle,
                ItemBonuses = new List<ItemBonus>()
                {
                    new ItemBonus() { ItemBonusTypeId = ItemBonusesType.Attack, Bonus = 3 }
                }
            });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, NpcTypeId = NpcsType.Farmer, Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Farmer) });
            await mayhemDataContext.SaveChangesAsync();

            await itemRepository.AssignItemToNpcAsync(newNpc.Entity.Id, newItem.Entity.Id, user.Entity.Id);
            double attributeValueBefore = (await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id)).Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).Select(x => x.Value).SingleOrDefault();

            bool status = await itemRepository.ReleaseItemFromNpcAsync(newItem.Entity.Id, user.Entity.Id);

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);


            status.Should().BeTrue();
            item.Npc.Should().BeNull();
            item.IsUsed.Should().BeFalse();
            npc.Attributes.Should().HaveCount(16);
            attributeValueBefore.Should().Be(1.03);
            npc.Attributes.Where(x => x.AttributeTypeId == AttributesType.Attack).SingleOrDefault().Value.Should().Be(1);
        }

        [Test]
        public async Task AssingAndReleaseItemFromNpcManyTime_WhenItemExist_ThenAttributesValueShouldBeCorrect_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item()
            {
                UserId = user.Entity.Id,
                IsMinted = true,
                ItemTypeId = ItemsType.Harvester,
                ItemBonuses = new List<ItemBonus>()
                {
                    new ItemBonus() { ItemBonusTypeId = ItemBonusesType.Wood, Bonus = 4 }
                }
            });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id, NpcTypeId = NpcsType.Miner, Attributes = AttributeHelper.GetBasicAttributesWithValue(NpcsType.Miner) });
            await mayhemDataContext.SaveChangesAsync();

            double lightWoodBonusBeforeRelease = 0;
            double heavyWoodBonusBeforeRelease = 0;


            for (int i = 0; i < 20; i++)
            {
                await itemRepository.AssignItemToNpcAsync(newNpc.Entity.Id, newItem.Entity.Id, user.Entity.Id);
                if (i == 0)
                {
                    lightWoodBonusBeforeRelease = (await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id)).Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).Select(x => x.Value).SingleOrDefault();
                    heavyWoodBonusBeforeRelease = (await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id)).Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).Select(x => x.Value).SingleOrDefault();
                }
                await itemRepository.ReleaseItemFromNpcAsync(newItem.Entity.Id, user.Entity.Id);
            }

            Item item = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == newItem.Entity.Id);
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == newNpc.Entity.Id);


            item.Npc.Should().BeNull();
            item.IsUsed.Should().BeFalse();
            npc.Attributes.Should().HaveCount(16);
            lightWoodBonusBeforeRelease.Should().Be(0.78);
            heavyWoodBonusBeforeRelease.Should().Be(0.416);
            npc.Attributes.Where(x => x.AttributeTypeId == AttributesType.LightWoodProduction).SingleOrDefault().Value.Should().Be(0.75);
            npc.Attributes.Where(x => x.AttributeTypeId == AttributesType.HeavyWoodProduction).SingleOrDefault().Value.Should().Be(0.4);
        }

        [Test]
        public async Task ReleaseItemAndCheckStateAfterAndBefor_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> item1 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Item> item2 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Item> item3 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Npc> npc1 = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            EntityEntry<Npc> npc2 = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<ItemDto> availableItems = await itemRepository.GetAvailableItemsByUserIdAsync(user.Entity.Id);
            IEnumerable<ItemDto> unavailableItems = await itemRepository.GetUnavailableItemsByUserIdAsync(user.Entity.Id);

            availableItems.Should().HaveCount(3);
            unavailableItems.Should().HaveCount(0);

            bool assignState1 = await itemRepository.AssignItemToNpcAsync(npc1.Entity.Id, item1.Entity.Id, user.Entity.Id);
            bool assignState2 = await itemRepository.AssignItemToNpcAsync(npc2.Entity.Id, item2.Entity.Id, user.Entity.Id);

            Item itemDb1 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item1.Entity.Id);
            Item itemDb2 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item2.Entity.Id);

            Npc npcDb1 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc1.Entity.Id);
            Npc npcDb2 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc2.Entity.Id);

            availableItems = await itemRepository.GetAvailableItemsByUserIdAsync(user.Entity.Id);
            unavailableItems = await itemRepository.GetUnavailableItemsByUserIdAsync(user.Entity.Id);

            assignState1.Should().BeTrue();
            assignState2.Should().BeTrue();

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

            bool releaseState1 = await itemRepository.ReleaseItemFromNpcAsync(item1.Entity.Id, user.Entity.Id);
            bool releaseState2 = await itemRepository.ReleaseItemFromNpcAsync(item2.Entity.Id, user.Entity.Id);

            itemDb1 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item1.Entity.Id);
            itemDb2 = await mayhemDataContext.Items.SingleOrDefaultAsync(x => x.Id == item2.Entity.Id);

            npcDb1 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc1.Entity.Id);
            npcDb2 = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npc2.Entity.Id);

            availableItems = await itemRepository.GetAvailableItemsByUserIdAsync(user.Entity.Id);
            unavailableItems = await itemRepository.GetUnavailableItemsByUserIdAsync(user.Entity.Id);

            releaseState1.Should().BeTrue();
            releaseState2.Should().BeTrue();

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
