using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AcceptInvitationByOwner
{
    public class AcceptInvitationByOwnerCommandHandler : IRequestHandler<AcceptInvitationByOwnerCommandRequest, AcceptInvitationByOwnerCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public AcceptInvitationByOwnerCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<AcceptInvitationByOwnerCommandResponse> Handle(AcceptInvitationByOwnerCommandRequest request, CancellationToken cancellationToken)
        {
            AddUserToGuildDto addUserToGuild = await guildRepository.AcceptInvitationByOwnerAsync(request.InvitationId, request.UserId);

            return new AcceptInvitationByOwnerCommandResponse()
            {
                AddUserToGuild = addUserToGuild,
            };
        }
    }
}
