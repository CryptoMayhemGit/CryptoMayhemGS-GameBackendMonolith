using MediatR;

namespace Mayhen.Bl.Commands.GetLandStatus
{
    public class GetLandStatusCommandRequest : IRequest<GetLandStatusCommandResponse>
    {
        public long LandId { get; set; }

        public GetLandStatusCommandRequest(long landId)
        {
            LandId = landId;
        }
    }
}
