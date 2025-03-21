using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.RemoveUserFromGuildByOwner
{
    public class RemoveUserFromGuildByOwnerCommandHandler : IRequestHandler<RemoveUserFromGuildByOwnerCommandRequest, RemoveUserFromGuildByOwnerCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public RemoveUserFromGuildByOwnerCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<RemoveUserFromGuildByOwnerCommandResponse> Handle(RemoveUserFromGuildByOwnerCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await guildRepository.RemoveUserFromGuildByOwnerAsync(request.RemovedUserId, request.UserId);

            return new RemoveUserFromGuildByOwnerCommandResponse()
            {
                Result = result,
            };
        }
    }
}
