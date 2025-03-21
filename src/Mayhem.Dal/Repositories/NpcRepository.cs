using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.Nfts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class NpcRepository : INpcRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public NpcRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<NpcDto> GetNpcByNftIdAsync(long heroNftId) => await mayhemDataContext
            .Npcs
            .AsNoTracking()
            .Where(x => x.Id == heroNftId)
            .Select(x => mapper.Map<NpcDto>(x))
            .SingleOrDefaultAsync();

        public async Task<NpcDto> GetUserNpcByIdWithAttributesAsync(long npcId, int userId) => await mayhemDataContext
            .Npcs
            .AsNoTracking()
            .Include(x => x.Attributes)
            .Where(x => x.Id == npcId && x.UserId == userId)
            .Select(x => mapper.Map<NpcDto>(x))
            .SingleOrDefaultAsync();

        public async Task<NpcDto> GetUserNpcByIdAsync(long npcId, int userId) => await mayhemDataContext
            .Npcs
            .AsNoTracking()
            .Where(x => x.Id == npcId && x.UserId == userId)
            .Select(x => mapper.Map<NpcDto>(x))
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<NpcDto>> GetAvailableNpcsByUserIdAsync(int userId) => await mayhemDataContext
            .Npcs
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.BuildingId == null)
            .Select(x => mapper.Map<NpcDto>(x))
            .ToListAsync();

        public async Task<bool> UpdateNpcHealthWithAttributesAsync(long npcId, ICollection<AttributeDto> attributes, NpcHealthsState newHealthsState)
        {
            Npc npc = await mayhemDataContext
                .Npcs
                .Include(x => x.Attributes)
                .SingleOrDefaultAsync(x => x.Id == npcId);

            npc.NpcHealthStateId = newHealthsState;
            foreach (AttributeDto attribute in attributes)
            {
                npc.Attributes.Where(x => x.AttributeTypeId == attribute.AttributeTypeId).SingleOrDefault().Value = attribute.Value;
            }

            await mayhemDataContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<NpcDto>> GetEnemyUserNpcsAsync(int userId, IEnumerable<long> userLandIds) => await mayhemDataContext
                .Npcs
                .AsNoTracking()
                .Where(x => x.UserId != userId && userLandIds.Contains(x.LandId.Value))
                .Select(x => mapper.Map<NpcDto>(x))
                .ToListAsync();

        public async Task<IEnumerable<NpcDto>> GetUserNpcsAsync(int userId) => await mayhemDataContext
                .Npcs
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => mapper.Map<NpcDto>(x))
                .ToListAsync();
    }
}
