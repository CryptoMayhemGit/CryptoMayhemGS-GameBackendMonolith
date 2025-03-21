using AutoMapper;
using Mayhem.Dal.Dto.Classes.Attributes;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Helper;
using Mayhem.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public ItemRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<ItemDto> GetItemByNftIdAsync(long itemNftId) => await mayhemDataContext
            .Items
            .AsNoTracking()
            .Where(x => x.Id == itemNftId)
            .Select(x => mapper.Map<ItemDto>(x))
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<ItemDto>> GetAvailableItemsByUserIdAsync(int userId) => await mayhemDataContext
            .Items
            .AsNoTracking()
            .Where(x => x.UserId == userId && !x.IsUsed)
            .Select(x => mapper.Map<ItemDto>(x))
            .ToListAsync();

        public async Task<IEnumerable<ItemDto>> GetUnavailableItemsByUserIdAsync(int userId) => await mayhemDataContext
            .Items
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.IsUsed)
            .Select(x => mapper.Map<ItemDto>(x))
            .ToListAsync();

        public async Task<bool> AssignItemToNpcAsync(long npcId, long itemId, int userId)
        {
            Item item = await mayhemDataContext
                .Items
                .Include(x => x.ItemBonuses)
                .SingleOrDefaultAsync(x => x.Id == itemId && x.UserId == userId);

            Npc npc = await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == npcId && x.UserId == userId);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                item.IsUsed = true;
                item.Npc = npc;
                npc.Item = item;

                foreach (Tables.ItemBonus itemBonus in item.ItemBonuses)
                {
                    IEnumerable<AttributesType> attributes = AttributeBonusDictionary.GetAttributeTypesByItemBonusType(itemBonus.ItemBonusTypeId);
                    IEnumerable<Tables.Attribute> attributesToUpdate = npc.Attributes.Where(x => attributes.Contains(x.AttributeTypeId));
                    foreach (Tables.Attribute attribute in attributesToUpdate)
                    {
                        attribute.Value = BonusHelper.IncreaseBonusValue(itemBonus.Bonus, attribute.BaseValue, attribute.Value);
                    }
                }
                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(AssignItemToNpcAsync));
            }

            return true;
        }

        public async Task<bool> ReleaseItemFromNpcAsync(long itemId, int userId)
        {
            Item item = await mayhemDataContext
                .Items
                .Include(x => x.ItemBonuses)
                .Include(x => x.Npc)
                .SingleOrDefaultAsync(x => x.Id == itemId && x.UserId == userId);

            Npc npc = await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == item.Npc.Id);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                item.IsUsed = false;
                item.Npc = null;

                foreach (Tables.ItemBonus itemBonus in item.ItemBonuses)
                {
                    IEnumerable<AttributesType> attributes = AttributeBonusDictionary.GetAttributeTypesByItemBonusType(itemBonus.ItemBonusTypeId);
                    List<Tables.Attribute> attributesToUpdate = npc.Attributes.Where(x => attributes.Contains(x.AttributeTypeId)).ToList();
                    foreach (Tables.Attribute attribute in attributesToUpdate)
                    {
                        attribute.Value = BonusHelper.DecreaseBonusValue(itemBonus.Bonus, attribute.BaseValue, attribute.Value);
                    }
                }

                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(ReleaseItemFromNpcAsync));
            }

            return true;
        }
    }
}
