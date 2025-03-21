using Mayhem.Dal.Dto.Classes.Buildings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetBuildingList
{
    public class GetBuildingListCommandHandler : IRequestHandler<GetBuildingListCommandRequest, GetBuildingListCommandResponse>
    {
        public async Task<GetBuildingListCommandResponse> Handle(GetBuildingListCommandRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new GetBuildingListCommandResponse()
            {
                Buildings = BuildingLandValidationDictionary.BuildingsPerSlot[request.LandTypeId]
            });
        }
    }
}
