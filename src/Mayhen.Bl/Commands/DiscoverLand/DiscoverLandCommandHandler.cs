using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.DiscoverLand
{
    public class DiscoverLandCommandHandler : IRequestHandler<DiscoverLandCommandRequest, DiscoverLandCommandResponse>
    {
        private readonly IDiscoveryMissionRepository discoveryMissionRepository;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public DiscoverLandCommandHandler(
            IDiscoveryMissionRepository discoveryMissionRepository,
            IMayhemConfigurationService mayhemConfigurationService)
        {
            this.discoveryMissionRepository = discoveryMissionRepository;
            this.mayhemConfigurationService = mayhemConfigurationService;
        }

        public async Task<DiscoverLandCommandResponse> Handle(DiscoverLandCommandRequest request, CancellationToken cancellationToken)
        {
            DiscoveryMissionDto discoveryMission = await discoveryMissionRepository.DiscoverMissionAsync(new DiscoveryMissionDto()
            {
                LandId = request.LandId,    
                NpcId = request.NpcId,
                UserId = request.UserId,
                FinishDate = DateTime.UtcNow.AddSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.DiscovertyMissionSpeedInSeconds),
            });

            return new DiscoverLandCommandResponse()
            {
                DiscoveryMission = discoveryMission,
            };
        }
    }
}
