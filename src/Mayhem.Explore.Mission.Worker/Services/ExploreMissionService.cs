using Mayhem.Explore.Mission.Worker.Interfaces;
using Mayhem.Messages;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Worker.Dal.Dto.Enums;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Explore.Mission.Worker.Services
{
    public class ExploreMissionService : IExploreMissionService
    {
        private readonly IExploreMissionRepository exploreMissionRepository;
        private readonly IUserLandRepository userLandRepository;
        private readonly INpcRepository npcRepository;
        private readonly ILogger<ExploreMissionService> logger;

        public ExploreMissionService(
            IExploreMissionRepository exploreMissionRepository,
            IUserLandRepository userLandRepository,
            INpcRepository npcRepository,
            ILogger<ExploreMissionService> logger)
        {
            this.exploreMissionRepository = exploreMissionRepository;
            this.userLandRepository = userLandRepository;
            this.npcRepository = npcRepository;
            this.logger = logger;
        }

        public async Task StartWorkAsync()
        {
            IEnumerable<ExploreMissionDto> missions = await exploreMissionRepository.GetFinishedMissionsAsync();

            if (!missions.Any())
            {
                return;
            }

            logger.LogDebug(LoggerMessages.FoundMissionsCount(missions.Count()));

            foreach (ExploreMissionDto mission in missions)
            {
                UserLandDto existingUserLand = await userLandRepository.GetUserLandAsync(mission.UserId, mission.LandId);

                if (existingUserLand != null)
                {
                    await userLandRepository.UpdateUserLandStatusAsync(existingUserLand.Id, LandsStatus.Explored);
                    logger.LogDebug(LoggerMessages.UpdatedUserLandWithExplored(existingUserLand.UserId, existingUserLand.LandId));
                }
                else
                {
                    UserLandDto userLand = new()
                    {
                        UserId = mission.UserId,
                        Status = LandsStatus.Explored,
                        Owned = false,
                        LandId = mission.LandId,
                        OnSale = false,
                        HasFog = false,
                    };

                    await userLandRepository.AddUserLandAsync(userLand);
                    logger.LogDebug(LoggerMessages.AddedUserLandExplored(userLand.UserId, userLand.LandId));
                }

                await npcRepository.UpdateNpcStatusAsync(mission.NpcId, NpcsStatus.None);
                await exploreMissionRepository.RemoveMissionAsync(mission.Id);
                logger.LogDebug(LoggerMessages.RemovedMissionWithId(mission.Id));
            }
        }
    }
}
