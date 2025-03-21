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
    public class DiscoveryMissionRepository : IDiscoveryMissionRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public DiscoveryMissionRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<DiscoveryMissionDto> DiscoverMissionAsync(DiscoveryMissionDto discoveryMissionDto)
        {
            EntityEntry<DiscoveryMission> discoveryMission = await mayhemDataContext.DiscoveryMissions.AddAsync(mapper.Map<DiscoveryMission>(discoveryMissionDto));
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == discoveryMissionDto.NpcId);
            npc.NpcStatusId = NpcsStatus.OnDiscoveryMission;
            await mayhemDataContext.SaveChangesAsync();
            return mapper.Map<DiscoveryMissionDto>(discoveryMission.Entity);
        }
    }
}
