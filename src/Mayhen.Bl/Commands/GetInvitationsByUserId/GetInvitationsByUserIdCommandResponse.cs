using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetInvitationsByUserId
{
    public class GetInvitationsByUserIdCommandResponse
    {
        public IEnumerable<GuildInvitationDto> Invitations { get; set; }
    }
}
