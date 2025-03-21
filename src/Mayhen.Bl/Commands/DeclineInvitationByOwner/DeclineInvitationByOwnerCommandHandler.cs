using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.DeclineInvitationByOwner
{
    public class DeclineInvitationByOwnerCommandHandler : IRequestHandler<DeclineInvitationByOwnerCommandRequest, DeclineInvitationByOwnerCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public DeclineInvitationByOwnerCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<DeclineInvitationByOwnerCommandResponse> Handle(DeclineInvitationByOwnerCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await guildRepository.DeclineInvitationByOwnerAsync(request.InvitationId);

            return new DeclineInvitationByOwnerCommandResponse()
            {
                Result = result,
            };
        }
    }
}
