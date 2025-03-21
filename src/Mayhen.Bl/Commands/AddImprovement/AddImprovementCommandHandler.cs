using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AddImprovement
{
    public class AddImprovementCommandHandler : IRequestHandler<AddImprovementCommandRequest, AddImprovementCommandResponse>
    {
        private readonly IImprovementRepository improvementRepository;
        private readonly IMapper mapper;

        public AddImprovementCommandHandler(
            IImprovementRepository improvementRepository,
            IMapper mapper)
        {
            this.improvementRepository = improvementRepository;
            this.mapper = mapper;
        }

        public async Task<AddImprovementCommandResponse> Handle(AddImprovementCommandRequest request, CancellationToken cancellationToken)
        {
            ImprovementDto improvement = await improvementRepository.AddImprovementAsync(mapper.Map<ImprovementDto>(request), request.UserId);

            return new AddImprovementCommandResponse()
            {
                Improvement = improvement
            };
        }
    }
}
