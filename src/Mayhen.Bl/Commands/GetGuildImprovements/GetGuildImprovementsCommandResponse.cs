using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetGuildImprovements
{
    public class GetGuildImprovementsCommandResponse
    {
        public IEnumerable<GuildImprovementDto> GuildImprovements { get; set; }
    }
}
