using Mayhem.Common.Services.PathFindingService.Dtos;
using Mayhem.Common.Services.PathFindingService.Interfaces;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.MoveNpc
{
    public class MoveNpcCommandHandler : PathCommandRequestHandler<MoveNpcCommandRequest, MoveNpcCommandResponse>
    {
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IPathFindingService pathFindingService;
        private readonly ITravelRepository travelRepository;
        private readonly IUserLandRepository userLandRepository;
        private readonly INpcRepository npcRepository;
        private readonly ILandRepository landRepository;

        public MoveNpcCommandHandler(
            IMayhemConfigurationService mayhemConfigurationService,
            ITravelRepository travelRepository,
            INpcRepository npcRepository,
            IUserLandRepository userLandRepository,
            IPathFindingService pathFindingService,
            ILandRepository landRepository)
        {
            this.travelRepository = travelRepository;
            this.npcRepository = npcRepository;
            this.userLandRepository = userLandRepository;
            this.pathFindingService = pathFindingService;
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.landRepository = landRepository;
        }

        public override async Task<MoveNpcCommandResponse> Handle(MoveNpcCommandRequest request, CancellationToken cancellationToken)
        {
            NpcDto npc = await npcRepository.GetNpcByNftIdAsync(request.NpcId);
            LandDto landFrom = await landRepository.GetLandByNftIdAsync(npc.LandId.Value);
            LandDto landTo = await landRepository.GetLandByNftIdAsync(request.LandToId);
            IEnumerable<LandPositionDto> userLands = await userLandRepository.GetUserLandsFromUserPerspectiveAsync(request.UserId, landFrom.LandInstanceId);
            List<PathLand> pathLands = pathFindingService.Calculate(CreateGridFromLands(userLands.ToArray(), mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.PlanetSize), new PathLand(landFrom.PositionX, landFrom.PositionY), new PathLand(landTo.PositionX, landTo.PositionY));

            List<long> pathLandIds = new();

            if (pathLands == null || pathLands.Count == 0)
            {
                throw new ValidationException(ValidationMessages.CouldNotGeneratePathForLands(landFrom.Id, landTo.Id));
            }

            pathLandIds.Add(landFrom.Id);
            for (int i = 0; i < pathLands.Count; i++)
            {
                pathLandIds.Add(userLands.Where(x => x.PositionX == pathLands[i].X && x.PositionY == pathLands[i].Y).Select(x => x.Id).Single());
            }
            
            List<TravelDto> travels = new();
            DateTime finishDate = DateTime.UtcNow;

            for (int i = 0; i < pathLandIds.Count - 1; i++)
            {
                finishDate = finishDate.AddSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NpcMoveSpeedInSeconds);
                travels.Add(new TravelDto()
                {
                    NpcId = npc.Id,
                    LandFromId = pathLandIds[i],
                    LandToId = pathLandIds[i + 1],
                    LandTargetId = request.LandToId,
                    FinishDate = finishDate,
                });
            }

            IEnumerable<TravelDto> addedTravels = await travelRepository.AddTravelsAsync(travels);

            return new MoveNpcCommandResponse()
            {
                Result = addedTravels.Any(),
            };
        }
    }
}
