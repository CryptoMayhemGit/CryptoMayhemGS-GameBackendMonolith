using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.ExploreLand
{
    public class ExploreLandCommandHandler : IRequestHandler<ExploreLandCommandRequest, ExploreLandCommandResponse>
    {
        private readonly IExploreMissionRepository exploreMissionRepository;
        private readonly IMayhemConfigurationService mayhemConfigurationService;

        public ExploreLandCommandHandler(
            IExploreMissionRepository exploreMissionRepository,
            IMayhemConfigurationService mayhemConfigurationService)
        {
            this.exploreMissionRepository = exploreMissionRepository;
            this.mayhemConfigurationService = mayhemConfigurationService;
        }

        public async Task<ExploreLandCommandResponse> Handle(ExploreLandCommandRequest request, CancellationToken cancellationToken)
        {
            ExploreMissionDto exploreMission = await exploreMissionRepository.ExploreMissionAsync(new ExploreMissionDto()
            {
                LandId = request.LandId,
                NpcId = request.NpcId,
                UserId = request.UserId,
                FinishDate = DateTime.UtcNow.AddSeconds(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.ExploreMissionSpeedInSeconds),
            });

            return new ExploreLandCommandResponse()
            {
                ExploreMission = exploreMission,
            };
        }
    }
}
