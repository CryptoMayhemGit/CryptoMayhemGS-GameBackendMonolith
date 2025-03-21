using MediatR;

namespace Mayhen.Bl.Commands.GetUserLands
{
    public class GetUserLandsCommandRequest : IRequest<GetUserLandsCommandResponse>
    {
        public int UserId { get; set; }

        public GetUserLandsCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
