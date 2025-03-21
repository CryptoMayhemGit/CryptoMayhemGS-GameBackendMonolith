using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AddGuildImprovement
{
    public class AddGuildImprovementCommandHandler : IRequestHandler<AddGuildImprovementCommandRequest, AddGuildImprovementCommandResponse>
    {
        private readonly IImprovementRepository improvementRepository;
        private readonly IMapper mapper;

        public AddGuildImprovementCommandHandler(
            IImprovementRepository improvementRepository,
            IMapper mapper)
        {
            this.improvementRepository = improvementRepository;
            this.mapper = mapper;
        }

        public async Task<AddGuildImprovementCommandResponse> Handle(AddGuildImprovementCommandRequest request, CancellationToken cancellationToken)
        {
            GuildImprovementDto guildImprovement = await improvementRepository.AddGuildImprovementAsync(mapper.Map<GuildImprovementDto>(request));

            return new AddGuildImprovementCommandResponse()
            {
                GuildImprovement = guildImprovement
            };
        }
    }
}
