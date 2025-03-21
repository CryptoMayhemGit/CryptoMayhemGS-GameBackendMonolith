using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Missions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using Mayhem.Dal.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Dal.Dto.Enums.Dictionaries;

namespace Mayhem.Dal.Repositories
{
    public class ExploreMissionRepository : IExploreMissionRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public ExploreMissionRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<ExploreMissionDto> ExploreMissionAsync(ExploreMissionDto exploreMissionDto)
        {
            EntityEntry<ExploreMission> exploreMission = await mayhemDataContext.ExploreMissions.AddAsync(mapper.Map<ExploreMission>(exploreMissionDto));
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == exploreMissionDto.NpcId);
            npc.NpcStatusId = NpcsStatus.OnExploreMission;
            await mayhemDataContext.SaveChangesAsync();
            return mapper.Map<ExploreMissionDto>(exploreMission.Entity);
        }
    }
}
