using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetUserLands
{
    public class GetUserLandsCommandHandler : IRequestHandler<GetUserLandsCommandRequest, GetUserLandsCommandResponse>
    {
        private readonly IUserLandRepository userLandRepository;
        private readonly IBuildingRepository buildingRepository;
        private readonly INpcRepository npcRepository;

        public GetUserLandsCommandHandler(
            IUserLandRepository userLandRepository,
            IBuildingRepository buildingRepository,
            INpcRepository npcRepository)
        {
            this.userLandRepository = userLandRepository;
            this.buildingRepository = buildingRepository;
            this.npcRepository = npcRepository;
        }

        public async Task<GetUserLandsCommandResponse> Handle(GetUserLandsCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<UserLandDto> userLands = await userLandRepository.GetUserLandsAsync(request.UserId);
            IEnumerable<long> userLandIds = userLands.Where(x => !x.HasFog).Select(x => x.Land.Id);
            IEnumerable<UserLandDto> ownedLands = userLands.Where(x => x.Owned);
            IEnumerable<BuildingDto> userBuildings = await buildingRepository.GetBuildingsByUserIdAsync(request.UserId);
            IEnumerable<BuildingDto> enemyBuildings = await buildingRepository.GetEnemyBuildingsByUserIdAsync(request.UserId, userLandIds);
            IEnumerable<UserLandDto> enemyLands = await userLandRepository.GetEnemyUserLandsAsync(request.UserId, userLandIds);
            IEnumerable<NpcDto> userNpcs = await npcRepository.GetUserNpcsAsync(request.UserId);
            IEnumerable<NpcDto> enemyNpcs = await npcRepository.GetEnemyUserNpcsAsync(request.UserId, userLandIds);
            
            return new GetUserLandsCommandResponse()
            {
                UserLands = userLands,
                OwnedLands = ownedLands,
                UserBuildings = userBuildings,
                EnemyBuildings = enemyBuildings,
                EnemyLands = enemyLands,
                UserNpcs = userNpcs,
                EnemyNpcs = enemyNpcs,
            };
        }
    }
}
