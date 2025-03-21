using MediatR;

namespace Mayhen.Bl.Commands.GetNftNpc
{
    public class GetNftNpcCommandRequest : IRequest<GetNftNpcCommandResponse>
    {
        public int HeroNftId { get; set; }

        public GetNftNpcCommandRequest(int heroNftId)
        {
            HeroNftId = heroNftId;
        }
    }
}
