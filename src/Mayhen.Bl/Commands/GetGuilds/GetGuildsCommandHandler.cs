using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetGuilds
{
    public class GetGuildsCommandHandler : IRequestHandler<GetGuildsCommandRequest, GetGuildsCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public GetGuildsCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<GetGuildsCommandResponse> Handle(GetGuildsCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<GuildDto> guilds = await guildRepository.GetGuildsAsync(request.Skip, request.Limit, request.Name);

            return new GetGuildsCommandResponse()
            {
                Guilds = guilds,
            };
        }
    }
}
