using Mayhem.Discovery.Mission.Worker.Interfaces;
using Mayhem.Messages;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Discovery.Mission.Worker.Services
{
    public class DiscoveryMissionService : IDiscoveryMissionService
    {
        private readonly IDiscoveryMissionRepository discoveryMissionRepository;
        private readonly IUserLandRepository userLandRepository;
        private readonly INpcRepository npcRepository;
        private readonly ILogger<DiscoveryMissionService> logger;

        public DiscoveryMissionService(
            IDiscoveryMissionRepository discoveryMissionRepository,
            IUserLandRepository userLandRepository,
            INpcRepository npcRepository,
            ILogger<DiscoveryMissionService> logger)
        {
            this.discoveryMissionRepository = discoveryMissionRepository;
            this.userLandRepository = userLandRepository;
            this.npcRepository = npcRepository;
            this.logger = logger;
        }

        public async Task StartWorkAsync()
        {
            IEnumerable<DiscoveryMissionDto> missions = await discoveryMissionRepository.GetFinishedMissionsAsync();

            if (!missions.Any())
            {
                return;
            }

            logger.LogDebug(LoggerMessages.FoundMissionsCount(missions.Count()));

            foreach (DiscoveryMissionDto mission in missions)
            {
                UserLandDto existingUserLand = await userLandRepository.GetUserLandAsync(mission.UserId, mission.LandId);

                if (existingUserLand != null)
                {
                    await userLandRepository.UpdateUserLandStatusAsync(existingUserLand.Id, LandsStatus.Discovered);
                    logger.LogDebug(LoggerMessages.UpdatedUserLandWithDiscovered(existingUserLand.UserId, existingUserLand.LandId));
                }
                else
                {
                    UserLandDto userLand = new()
                    {
                        UserId = mission.UserId,
                        Status = LandsStatus.Discovered,
                        Owned = false,
                        LandId = mission.LandId,
                        OnSale = false,
                        HasFog = false,
                    };

                    await userLandRepository.AddUserLandAsync(userLand);
                    logger.LogDebug(LoggerMessages.AddedUserLandDiscovered(userLand.UserId, userLand.LandId));
                }

                await npcRepository.UpdateNpcStatusAsync(mission.NpcId, NpcsStatus.None);
                await discoveryMissionRepository.RemoveMissionAsync(mission.Id);
                logger.LogDebug(LoggerMessages.RemovedMissionWithId(mission.Id));
            }
        }
    }
}
