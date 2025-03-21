using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.ChangeGuildOwner
{
    public class ChangeGuildOwnerCommandHandler : IRequestHandler<ChangeGuildOwnerCommandRequest, ChangeGuildOwnerCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public ChangeGuildOwnerCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<ChangeGuildOwnerCommandResponse> Handle(ChangeGuildOwnerCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await guildRepository.ChangeGuildOwnerAsync(request.NewOwnerId, request.UserId);

            return new ChangeGuildOwnerCommandResponse()
            {
                Result = result,
            };
        }
    }
}
