using MediatR;

namespace Mayhen.Bl.Commands.GetNftItem
{
    public class GetNftItemCommandRequest : IRequest<GetNftItemCommandResponse>
    {
        public int ItemNftId { get; set; }

        public GetNftItemCommandRequest(int itemNftId)
        {
            ItemNftId = itemNftId;
        }
    }
}
