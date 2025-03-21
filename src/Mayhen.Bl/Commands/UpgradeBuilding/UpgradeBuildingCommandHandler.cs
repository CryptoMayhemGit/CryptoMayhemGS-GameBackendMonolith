using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.UpgradeBuilding
{
    public class UpgradeBuildingCommandHandler : IRequestHandler<UpgradeBuildingCommandRequest, UpgradeBuildingCommandResponse>
    {
        private readonly IBuildingRepository buildingRepository;

        public UpgradeBuildingCommandHandler(IBuildingRepository buildingRepository)
        {
            this.buildingRepository = buildingRepository;
        }

        public async Task<UpgradeBuildingCommandResponse> Handle(UpgradeBuildingCommandRequest request, CancellationToken cancellationToken)
        {
            BuildingDto upgradedBuilding = await buildingRepository.UpgradeBuildingAsync(request.BuildingId, request.UserId);

            return new UpgradeBuildingCommandResponse()
            {
                Building = upgradedBuilding,
            };
        }
    }
}
