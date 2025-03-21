using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetInvitationsByGuildId
{
    public class GetInvitationsByGuildIdCommandHandler : IRequestHandler<GetInvitationsByGuildIdCommandRequest, GetInvitationsByGuildIdCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public GetInvitationsByGuildIdCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<GetInvitationsByGuildIdCommandResponse> Handle(GetInvitationsByGuildIdCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<GuildInvitationDto> invitations = await guildRepository.GetInvitationsByGuildIdAsync(request.GuildId, request.UserId);

            return new GetInvitationsByGuildIdCommandResponse()
            {
                Invitations = invitations,
            };
        }
    }
}
