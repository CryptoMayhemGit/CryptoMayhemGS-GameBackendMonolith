using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AddBuildingToLand
{
    public class AddBuildingToLandCommandHandler : IRequestHandler<AddBuildingToLandCommandRequest, AddBuildingToLandCommandResponse>
    {
        private readonly IBuildingRepository buildingRepository;

        public AddBuildingToLandCommandHandler(IBuildingRepository buildingRepository)
        {
            this.buildingRepository = buildingRepository;
        }

        public async Task<AddBuildingToLandCommandResponse> Handle(AddBuildingToLandCommandRequest request, CancellationToken cancellationToken)
        {
            BuildingDto building = await buildingRepository.AddBuildingToLandAsync(request.LandId, request.BuildingTypeId, request.UserId);

            return new AddBuildingToLandCommandResponse()
            {
                Building = building,
            };
        }
    }
}
