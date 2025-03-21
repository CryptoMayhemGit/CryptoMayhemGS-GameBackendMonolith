using MediatR;

namespace Mayhen.Bl.Commands.GetGuildImprovements
{
    public class GetGuildImprovementsCommandRequest : IRequest<GetGuildImprovementsCommandResponse>
    {
        public int GuildId { get; set; }

        public GetGuildImprovementsCommandRequest(int guildId)
        {
            GuildId = guildId;
        }
    }
}
