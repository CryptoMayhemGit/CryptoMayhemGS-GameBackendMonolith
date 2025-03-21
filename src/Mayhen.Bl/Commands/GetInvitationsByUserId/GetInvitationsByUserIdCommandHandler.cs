using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetInvitationsByUserId
{
    public class GetInvitationsByUserIdCommandHandler : IRequestHandler<GetInvitationsByUserIdCommandRequest, GetInvitationsByUserIdCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public GetInvitationsByUserIdCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<GetInvitationsByUserIdCommandResponse> Handle(GetInvitationsByUserIdCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<GuildInvitationDto> invitations = await guildRepository.GetInvitationsByUserIdAsync(request.UserId);

            return new GetInvitationsByUserIdCommandResponse()
            {
                Invitations = invitations,
            };
        }
    }
}
