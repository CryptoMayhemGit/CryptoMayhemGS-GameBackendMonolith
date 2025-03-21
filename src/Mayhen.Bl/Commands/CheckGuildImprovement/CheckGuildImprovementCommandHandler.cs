using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhen.Bl.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CheckGuildImprovement
{
    public class CheckGuildImprovementCommandHandler : IRequestHandler<CheckGuildImprovementCommandRequest, CheckGuildImprovementCommandResponse>
    {
        private readonly IImprovementRepository improvementRepository;
        private readonly IGuildImprovementValidationService guildImprovementValidationService;

        public CheckGuildImprovementCommandHandler(
            IImprovementRepository improvementRepository,
            IGuildImprovementValidationService guildImprovementValidationService)
        {
            this.improvementRepository = improvementRepository;
            this.guildImprovementValidationService = guildImprovementValidationService;
        }

        public async Task<CheckGuildImprovementCommandResponse> Handle(CheckGuildImprovementCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<GuildImprovementDto> improvements = await improvementRepository.GetGuildImprovementsByGuildIdAsync(request.GuildId);
            bool canImprove = guildImprovementValidationService.ValidateImprovement(request.Level, request.GuildBuildingsTypeId, improvements.Select(x => x.GuildImprovementTypeId));

            return new CheckGuildImprovementCommandResponse()
            {
                CanImprove = canImprove,
            };
        }
    }
}
