using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetGuildById
{
    public class GetGuildByIdCommandHandler : IRequestHandler<GetGuildByIdCommandRequest, GetGuildByIdCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public GetGuildByIdCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<GetGuildByIdCommandResponse> Handle(GetGuildByIdCommandRequest request, CancellationToken cancellationToken)
        {
            GuildDto guild = await guildRepository.GetGuildByIdAsync(request.GuildId);

            return new GetGuildByIdCommandResponse()
            {
                Guild = guild,
            };
        }
    }
}
