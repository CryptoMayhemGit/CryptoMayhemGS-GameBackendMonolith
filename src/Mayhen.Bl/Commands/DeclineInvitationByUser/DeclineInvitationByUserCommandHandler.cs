using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.DeclineInvitationByUser
{
    public class DeclineInvitationByUserCommandHandler : IRequestHandler<DeclineInvitationByUserCommandRequest, DeclineInvitationByUserCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public DeclineInvitationByUserCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<DeclineInvitationByUserCommandResponse> Handle(DeclineInvitationByUserCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await guildRepository.DeclineInvitationByUserAsync(request.InvitationId);

            return new DeclineInvitationByUserCommandResponse()
            {
                Result = result,
            };
        }
    }
}
