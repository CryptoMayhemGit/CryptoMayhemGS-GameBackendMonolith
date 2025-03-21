using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.LeaveGuild
{
    public class LeaveGuildCommandHandler : IRequestHandler<LeaveGuildCommandRequest, LeaveGuildCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public LeaveGuildCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<LeaveGuildCommandResponse> Handle(LeaveGuildCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await guildRepository.LeaveGuildAsync(request.UserId);

            return new LeaveGuildCommandResponse()
            {
                Result = result,
            };
        }
    }
}
