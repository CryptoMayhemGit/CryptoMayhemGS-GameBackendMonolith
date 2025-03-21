using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;

namespace Mayhen.Bl.Commands.GetBuildingList
{
    public class GetBuildingListCommandRequest : IRequest<GetBuildingListCommandResponse>
    {
        public LandsType LandTypeId { get; set; }

        public GetBuildingListCommandRequest(LandsType landTypeId)
        {
            LandTypeId = landTypeId;
        }
    }
}
