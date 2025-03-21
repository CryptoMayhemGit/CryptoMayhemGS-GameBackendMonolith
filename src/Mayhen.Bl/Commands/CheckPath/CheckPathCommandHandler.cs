using Mayhem.Common.Services.PathFindingService.Dtos;
using Mayhem.Common.Services.PathFindingService.Interfaces;
using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhen.Bl.Commands.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CheckPath
{
    public class CheckPathCommandHandler : PathCommandRequestHandler<CheckPathCommandRequest, CheckPathCommandResponse>
    {
        private readonly IPathFindingService pathFindingService;
        private readonly IUserLandRepository userLandRepository;
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly ILandRepository landRepository;

        public CheckPathCommandHandler(
            IPathFindingService pathFindingService,
            IUserLandRepository userLandRepository,
            ILandRepository landRepository,
            IMayhemConfigurationService mayhemConfigurationService)
        {
            this.pathFindingService = pathFindingService;
            this.userLandRepository = userLandRepository;
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.landRepository = landRepository;
        }

        public override async Task<CheckPathCommandResponse> Handle(CheckPathCommandRequest request, CancellationToken cancellationToken)
        {
            LandDto landFrom = await landRepository.GetLandByNftIdAsync(request.LandFromId);
            LandDto landTo = await landRepository.GetLandByNftIdAsync(request.LandToId);
            IEnumerable<LandPositionDto> userLands = await userLandRepository.GetUserLandsFromUserPerspectiveAsync(request.UserId, landFrom.LandInstanceId);
            List<PathLand> pathLands = pathFindingService.Calculate(CreateGridFromLands(userLands.ToArray(), mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.PlanetSize), new PathLand(landFrom.PositionX, landFrom.PositionY), new PathLand(landTo.PositionX, landTo.PositionY));
            List<long> pathLandIds = new();

            if (pathLands == null || pathLands.Count > 0)
            {
                pathLandIds.Add(landFrom.Id);
                for (int i = 0; i < pathLands.Count; i++)
                {
                    pathLandIds.Add(userLands.Where(x => x.PositionX == pathLands[i].X && x.PositionY == pathLands[i].Y).Select(x => x.Id).Single());
                }
            }

            return new CheckPathCommandResponse()
            {
                PathPossible = pathLands.Count > 0,
                Time = pathLands.Count * mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NpcMoveSpeedInSeconds,
                Lands = pathLandIds,
            };
        }
    }
}
