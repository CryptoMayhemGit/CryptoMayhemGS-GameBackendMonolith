using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AcceptInvitationByUser
{
    public class AcceptInvitationByUserCommandHandler : IRequestHandler<AcceptInvitationByUserCommandRequest, AcceptInvitationByUserCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public AcceptInvitationByUserCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<AcceptInvitationByUserCommandResponse> Handle(AcceptInvitationByUserCommandRequest request, CancellationToken cancellationToken)
        {
            AddUserToGuildDto addUserToGuild = await guildRepository.AcceptInvitationByUserAsync(request.InvitationId, request.UserId);

            return new AcceptInvitationByUserCommandResponse()
            {
                AddUserToGuild = addUserToGuild,
            };
        }
    }
}
