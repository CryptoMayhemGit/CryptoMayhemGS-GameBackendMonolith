using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.UpgradeGuildBuilding
{
    public class UpgradeGuildBuildingCommandHandler : IRequestHandler<UpgradeGuildBuildingCommandRequest, UpgradeGuildBuildingCommandResponse>
    {
        private readonly IGuildBuildingRepository guildBuildingRepository;

        public UpgradeGuildBuildingCommandHandler(
            IGuildBuildingRepository guildBuildingRepository)
        {
            this.guildBuildingRepository = guildBuildingRepository;
        }

        public async Task<UpgradeGuildBuildingCommandResponse> Handle(UpgradeGuildBuildingCommandRequest request, CancellationToken cancellationToken)
        {

            GuildBuildingDto guildBuilding = await guildBuildingRepository.UpgradeGuildBuildingAsync(request.GuildBuildingId, request.UserId);

            return new UpgradeGuildBuildingCommandResponse()
            {
                GuildBuilding = guildBuilding,
            };
        }
    }
}
