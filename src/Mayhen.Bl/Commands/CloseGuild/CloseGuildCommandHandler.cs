using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CloseGuild
{
    public class CloseGuildCommandHandler : IRequestHandler<CloseGuildCommandRequest, CloseGuildCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public CloseGuildCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<CloseGuildCommandResponse> Handle(CloseGuildCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await guildRepository.CloseGuildAsync(request.UserId);

            return new CloseGuildCommandResponse()
            {
                Result = result,
            };
        }
    }
}
