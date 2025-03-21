using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CreateGuild
{
    public class CreateGuildCommandHandler : IRequestHandler<CreateGuildCommandRequest, CreateGuildCommandResponse>
    {
        private readonly IGuildRepository guildRepository;

        public CreateGuildCommandHandler(IGuildRepository guildRepository)
        {
            this.guildRepository = guildRepository;
        }

        public async Task<CreateGuildCommandResponse> Handle(CreateGuildCommandRequest request, CancellationToken cancellationToken)
        {
            GuildDto guild = await guildRepository.CreateGuildAsync(request.Name, request.Description, request.UserId);

            return new CreateGuildCommandResponse()
            {
                Guild = guild,
            };
        }
    }
}
