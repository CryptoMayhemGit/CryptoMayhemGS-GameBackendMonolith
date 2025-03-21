using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhen.Bl.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CheckImprovement
{
    public class CheckImprovementCommandHandler : IRequestHandler<CheckImprovementCommandRequest, CheckImprovementCommandResponse>
    {
        private readonly IImprovementRepository improvementRepository;
        private readonly IImprovementValidationService improvementValidator;

        public CheckImprovementCommandHandler(IImprovementRepository improvementRepository, IImprovementValidationService improvementValidator)
        {
            this.improvementRepository = improvementRepository;
            this.improvementValidator = improvementValidator;
        }

        public async Task<CheckImprovementCommandResponse> Handle(CheckImprovementCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<ImprovementDto> improvements = await improvementRepository.GetImprovementsByLandIdAsync(request.LandId);
            bool canImprove = improvementValidator.ValidateImprovement(request.Level, request.BuildingsTypeId, improvements.Select(x => x.ImprovementTypeId));

            return new CheckImprovementCommandResponse()
            {
                CanImprove = canImprove,
            };
        }
    }
}
