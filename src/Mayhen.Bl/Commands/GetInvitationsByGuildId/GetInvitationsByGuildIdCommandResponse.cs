using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetInvitationsByGuildId
{
    public class GetInvitationsByGuildIdCommandResponse
    {
        public IEnumerable<GuildInvitationDto> Invitations { get; set; }
    }
}
