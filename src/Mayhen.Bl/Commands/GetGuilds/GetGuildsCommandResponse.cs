using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetGuilds
{
    public class GetGuildsCommandResponse
    {
        public IEnumerable<GuildDto> Guilds { get; set; }
    }
}
