using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AsksToJoinGuildByUser
{
    public class AskToJoinGuildByUserCommandHandler : IRequestHandler<AskToJoinGuildByUserCommandRequest, AskToJoinGuildByUserCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public AskToJoinGuildByUserCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<AskToJoinGuildByUserCommandResponse> Handle(AskToJoinGuildByUserCommandRequest request, CancellationToken cancellationToken)
        {
            InviteUserDto inviteUser = await guildRepository.AskToJoinGuildByUserAsync(request.GuildId, request.UserId);

            return new AskToJoinGuildByUserCommandResponse()
            {
                InviteUser = inviteUser,
            };
        }
    }
}
