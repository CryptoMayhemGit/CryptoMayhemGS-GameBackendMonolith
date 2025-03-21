using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetGuildBuildingList
{
    public class GetGuildBuildingListCommandHandler : IRequestHandler<GetGuildBuildingListCommandRequest, GetGuildBuildingListCommandResponse>
    {
        private readonly IGuildBuildingRepository guildBuildingRepository;

        public GetGuildBuildingListCommandHandler(IGuildBuildingRepository guildBuildingRepository)
        {
            this.guildBuildingRepository = guildBuildingRepository;
        }

        public async Task<GetGuildBuildingListCommandResponse> Handle(GetGuildBuildingListCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<GuildBuildingDto> guildBuildings = await guildBuildingRepository.GetGuildBuildingsByGuildIdAsync(request.GuildId);

            return new GetGuildBuildingListCommandResponse()
            {
                GuildBuildings = guildBuildings,
            };
        }
    }
}
