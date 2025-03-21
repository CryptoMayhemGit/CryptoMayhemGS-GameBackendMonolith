using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetImprovements
{
    public class GetImprovementsCommandHandler : IRequestHandler<GetImprovementsCommandRequest, GetImprovementsCommandResponse>
    {
        private readonly IImprovementRepository improvementRepository;

        public GetImprovementsCommandHandler(IImprovementRepository improvementRepository)
        {
            this.improvementRepository = improvementRepository;
        }

        public async Task<GetImprovementsCommandResponse> Handle(GetImprovementsCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<ImprovementDto> improvements = await improvementRepository.GetImprovementsByLandIdAsync(request.LandId);

            return new GetImprovementsCommandResponse()
            {
                Improvements = improvements,
            };
        }
    }
}
