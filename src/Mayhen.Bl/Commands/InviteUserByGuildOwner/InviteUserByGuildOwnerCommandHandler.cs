using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.InviteUserByGuildOwner
{
    public class InviteUserByGuildOwnerCommandHandler : IRequestHandler<InviteUserByGuildOwnerCommandRequest, InviteUserByGuildOwnerCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public InviteUserByGuildOwnerCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<InviteUserByGuildOwnerCommandResponse> Handle(InviteUserByGuildOwnerCommandRequest request, CancellationToken cancellationToken)
        {
            InviteUserDto inviteUser = await guildRepository.InviteUserByGuildOwnerAsync(request.InvitedUserId, request.UserId);

            return new InviteUserByGuildOwnerCommandResponse()
            {
                InviteUser = inviteUser,
            };
        }
    }
}
