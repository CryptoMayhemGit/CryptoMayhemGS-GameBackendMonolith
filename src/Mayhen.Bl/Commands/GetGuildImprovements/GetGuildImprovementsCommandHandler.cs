using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetGuildImprovements
{
    public class GetGuildImprovementsCommandHandler : IRequestHandler<GetGuildImprovementsCommandRequest, GetGuildImprovementsCommandResponse>
    {
        private readonly IImprovementRepository improvementRepository;

        public GetGuildImprovementsCommandHandler(IImprovementRepository improvementRepository)
        {
            this.improvementRepository = improvementRepository;
        }

        public async Task<GetGuildImprovementsCommandResponse> Handle(GetGuildImprovementsCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<GuildImprovementDto> guildImprovements = await improvementRepository.GetGuildImprovementsByGuildIdAsync(request.GuildId);

            return new GetGuildImprovementsCommandResponse()
            {
                GuildImprovements = guildImprovements,
            };
        }
    }
}
