using Mayhem.Common.Services.PathFindingService.Dtos;
using Mayhem.Common.Services.PathFindingService.Enums;
using Mayhem.Common.Services.PathFindingService.Interfaces;
using Mayhem.Configuration.Interfaces;
using Mayhem.Messages;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
using Mayhem.Worker.Path.Interfaces;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Worker.Path.Services
{
    public class PathWorkerService : IPathWorkerService
    {
        private readonly ITravelRepository travelRepository;
        private readonly ILandRepository landRepository;
        private readonly INpcRepository npcRepository;
        private readonly IUserLandRepository userLandRepository;
        private readonly IPathFindingService pathFindingService;
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly ILogger<PathWorkerService> logger;

        public PathWorkerService(
            ITravelRepository travelRepository,
            ILandRepository landRepository,
            INpcRepository npcRepository,
            IUserLandRepository userLandRepository,
            IPathFindingService pathFindingService,
            IMayhemConfigurationService mayhemConfigurationService,
            ILogger<PathWorkerService> logger)
        {
            this.travelRepository = travelRepository;
            this.npcRepository = npcRepository;
            this.landRepository = landRepository;
            this.userLandRepository = userLandRepository;
            this.pathFindingService = pathFindingService;
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.logger = logger;
        }

        public async Task StartWorkAsync()
        {
            IEnumerable<TravelDto> travels = await travelRepository.GetTravelsAsync();

            if (!travels.Any())
            {
                return;
            }

            logger.LogDebug(LoggerMessages.FoundTravelsCount(travels.Count()));

            foreach (TravelDto travel in travels)
            {
                NpcDto npc = await npcRepository.GetNpcAsync(travel.NpcId);
                IEnumerable<UserLandNpcDto> userLandNpcs = await landRepository.GetLandNpcsAsync(travel.LandToId);

                if (userLandNpcs.Any(x => x.LandUserId != npc.UserId && x.Owned) ||
                    userLandNpcs.Any(x => x.NpcUserId != null && x.NpcUserId != npc.UserId))
                {
                    IEnumerable<TravelDto> allTravels = await travelRepository.GetTravelsByNpcIdAsync(npc.Id);
                    await travelRepository.RemoveTravelsByNpcIdAsync(npc.Id);
                    IEnumerable<LandPositionDto> userLands = await userLandRepository.GetUserLandsFromUserPerspectiveAsync(npc.UserId.Value);
                    LandPositionDto landFrom = userLands.SingleOrDefault(x => x.Id == allTravels.First().LandFromId);
                    LandPositionDto landTo = userLands.SingleOrDefault(x => x.Id == allTravels.Last().LandToId);
                    List<PathLand> pathLands = pathFindingService.Calculate(
                        CreateGridFromLands(userLands.ToList(), mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.PlanetSize),
                        new PathLand(landFrom.PositionX, landFrom.PositionY),
                        new PathLand(landTo.PositionX, landTo.PositionY));
                    if (pathLands == null || pathLands.Count == 0 || pathLands.Count - 4 > allTravels.Count())
                    {
                        logger.LogDebug(LoggerMessages.EnemyWasEncounteredNewPathImpossible());
                        return;
                    }
                    List<long> pathLandIds = new();
                    pathLandIds.Add(landFrom.Id);
                    for (int i = 0; i < pathLands.Count; i++)
                    {
                        pathLandIds.Add(userLands.Where(x => x.PositionX == pathLands[i].X && x.PositionY == pathLands[i].Y).Select(x => x.Id).SingleOrDefault());
                    }

                    List<TravelDto> newTravels = new();
                    DateTime finishDate = DateTime.UtcNow;

                    for (int i = 0; i < pathLandIds.Count - 1; i++)
                    {
                        newTravels.Add(new TravelDto()
                        {
                            NpcId = npc.Id,
                            LandFromId = pathLandIds[i],
                            LandToId = pathLandIds[i + 1],
                            LandTargetId = landTo.Id,
                            FinishDate = finishDate,
                        });
                        finishDate = finishDate.AddSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.NpcMoveSpeedInSeconds);
                    }

                    await travelRepository.AddTravelsAsync(newTravels);
                    logger.LogDebug(LoggerMessages.EnemyWasEncounteredNewPathCalculated());
                }
                else
                {
                    await npcRepository.UpdateNpcLandAsync(npc.Id, travel.LandToId);
                    await landRepository.AddFogToLandsAsync(travel.LandFromId, npc.UserId.Value, npc.Id);
                    await landRepository.RemoveFogFromLandsAsync(travel.LandToId, npc.UserId.Value);
                    await travelRepository.RemoveTravelAsync(travel.Id);

                    if (travel.LandTargetId == travel.LandToId)
                    {
                        await npcRepository.UpdateNpcStatusAsync(travel.NpcId, NpcsStatus.None);
                        logger.LogDebug(LoggerMessages.TravelForNpcWithIdFinished(travel.NpcId));
                    }

                    logger.LogDebug(LoggerMessages.TravelFromLandToLandForNpcEndedUnhindered(travel.LandFromId, travel.LandToId, travel.NpcId));
                }
            }
        }

        private int[,] CreateGridFromLands(List<LandPositionDto> lands, int arraySize)
        {
            int[,] planetLandArray = new int[arraySize, arraySize];

            for (int i = 0; i < lands.Count; i++)
            {
                planetLandArray[lands[i].PositionX, lands[i].PositionY] = (int)PathFindingLandsType.PATH;
            }

            return planetLandArray;
        }
    }
}
