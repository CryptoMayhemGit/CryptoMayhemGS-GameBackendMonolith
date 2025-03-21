using MediatR;

namespace Mayhen.Bl.Commands.GetGuildBuildingList
{
    public class GetGuildBuildingListCommandRequest : IRequest<GetGuildBuildingListCommandResponse>
    {
        public int GuildId { get; set; }

        public GetGuildBuildingListCommandRequest(int guildId)
        {
            GuildId = guildId;
        }
    }
}
