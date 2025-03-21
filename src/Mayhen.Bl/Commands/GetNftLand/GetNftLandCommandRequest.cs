using MediatR;

namespace Mayhen.Bl.Commands.GetNftLand
{
    public class GetNftLandCommandRequest : IRequest<GetNftLandCommandResponse>
    {
        public int LandNftId { get; set; }

        public GetNftLandCommandRequest(int landNftId)
        {
            LandNftId = landNftId;
        }
    }
}
