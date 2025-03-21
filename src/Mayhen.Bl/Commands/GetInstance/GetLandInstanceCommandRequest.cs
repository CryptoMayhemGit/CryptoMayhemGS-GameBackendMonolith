using MediatR;

namespace Mayhen.Bl.Commands.GetInstance
{
    public class GetLandInstanceCommandRequest : IRequest<GetLandInstanceCommandResponse>
    {
        public int InstanceId { get; set; }

        public GetLandInstanceCommandRequest(int instanceId)
        {
            InstanceId = instanceId;
        }
    }
}
