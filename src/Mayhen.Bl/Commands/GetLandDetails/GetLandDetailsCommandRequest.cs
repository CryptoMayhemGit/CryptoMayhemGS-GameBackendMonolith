using MediatR;

namespace Mayhen.Bl.Commands.GetLandDetails
{
    public class GetLandDetailsCommandRequest : IRequest<GetLandDetailsCommandResponse>
    {
        public long LandId { get; set; }

        public GetLandDetailsCommandRequest(long landId)
        {
            LandId = landId;
        }
    }
}
