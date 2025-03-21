using FluentAssertions;
using FluentValidation.Results;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Commands.AssignItemToNpc;
using Mayhen.Bl.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Validators
{
    public class AssignItemToNpcCommandRequestValidatorTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IItemRepository itemRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            itemRepository = GetService<IItemRepository>();
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemAlreadyHasNpc_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Npc> newNpc1 = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            EntityEntry<Npc> newNpc2 = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            await itemRepository.AssignItemToNpcAsync(newNpc1.Entity.Id, newItem.Entity.Id, user.Entity.Id);

            AssignItemToNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AssignItemToNpcCommandRequest()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc2.Entity.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Item with id {newItem.Entity.Id} is already assigned.");
            result.Errors.First().PropertyName.Should().Be($"ItemId");
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemIsNotMinted_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            AssignItemToNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AssignItemToNpcCommandRequest()
            {
                ItemId = newItem.Entity.Id,
                NpcId = newNpc.Entity.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Item with id {newItem.Entity.Id} is not minted.");
            result.Errors.First().PropertyName.Should().Be($"ItemId");
        }

        [Test]
        public async Task AssignItemToNpc_WhenNpcNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> item = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            await mayhemDataContext.SaveChangesAsync();

            AssignItemToNpcCommandRequestValidator validator = new(mayhemDataContext);
            AssignItemToNpcCommandRequest request = new()
            {
                ItemId = item.Entity.Id,
                NpcId = 4353462,
                UserId = user.Entity.Id,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {request.NpcId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task AssignItemToNpc_WhenNpcAlreadyHasItem_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Item> newItem1 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Item> newItem2 = await mayhemDataContext.Items.AddAsync(new Item() { UserId = user.Entity.Id, IsMinted = true });
            EntityEntry<Npc> newNpc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            await itemRepository.AssignItemToNpcAsync(newNpc.Entity.Id, newItem1.Entity.Id, user.Entity.Id);

            AssignItemToNpcCommandRequestValidator validator = new(mayhemDataContext);
            ValidationResult result = validator.Validate(new AssignItemToNpcCommandRequest()
            {
                ItemId = newItem2.Entity.Id,
                NpcId = newNpc.Entity.Id,
                UserId = user.Entity.Id,
            });

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {newNpc.Entity.Id} has item.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemNotExist_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());
            EntityEntry<Npc> npc = await mayhemDataContext.Npcs.AddAsync(new Npc() { UserId = user.Entity.Id });
            await mayhemDataContext.SaveChangesAsync();

            AssignItemToNpcCommandRequestValidator validator = new(mayhemDataContext);
            AssignItemToNpcCommandRequest request = new()
            {
                ItemId = 1244321,
                NpcId = npc.Entity.Id,
                UserId = user.Entity.Id,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Item with id {request.ItemId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"ItemId");
        }

        [Test]
        public async Task AssignItemToNpc_WhenItemAndNpcNotExists_ThenThrowException_Test()
        {
            EntityEntry<GameUser> user = await mayhemDataContext.GameUsers.AddAsync(new GameUser());

            AssignItemToNpcCommandRequestValidator validator = new(mayhemDataContext);
            AssignItemToNpcCommandRequest request = new()
            {
                ItemId = 12343212,
                NpcId = 1342321,
                UserId = user.Entity.Id,
            };
            ValidationResult result = validator.Validate(request);

            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"Npc with id {request.NpcId} doesn't exist.");
            result.Errors.First().PropertyName.Should().Be($"NpcId");
        }
    }
}
