using MediatR;

namespace Mayhen.Bl.Commands.GetGuilds
{
    public class GetGuildsCommandRequest : IRequest<GetGuildsCommandResponse>
    {
        public int? Skip { get; set; }
        public int? Limit { get; set; }
        public string Name { get; set; }
    }
}
