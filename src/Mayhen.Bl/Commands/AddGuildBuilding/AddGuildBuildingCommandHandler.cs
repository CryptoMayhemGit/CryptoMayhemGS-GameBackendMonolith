using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AddGuildBuilding
{
    public class AddGuildBuildingCommandHandler : IRequestHandler<AddGuildBuildingCommandRequest, AddGuildBuildingCommandResponse>
    {
        private readonly IGuildBuildingRepository guildBuildingRepository;

        public AddGuildBuildingCommandHandler(IGuildBuildingRepository guildBuildingRepository)
        {
            this.guildBuildingRepository = guildBuildingRepository;
        }

        public async Task<AddGuildBuildingCommandResponse> Handle(AddGuildBuildingCommandRequest request, CancellationToken cancellationToken)
        {
            GuildBuildingDto guildBuilding = await guildBuildingRepository.AddGuildBuildingAsync(request.GuildId, request.GuildBuildingTypeId, request.UserId);

            return new AddGuildBuildingCommandResponse()
            {
                GuildBuilding = guildBuilding,
            };
        }
    }
}
