using MediatR;

namespace Mayhen.Bl.Commands.GetGuildById
{
    public class GetGuildByIdCommandRequest : IRequest<GetGuildByIdCommandResponse>
    {
        public int GuildId { get; set; }

        public GetGuildByIdCommandRequest(int guildId)
        {
            GuildId = guildId;
        }
    }
}
