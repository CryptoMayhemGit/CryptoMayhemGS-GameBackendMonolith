using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetAvailableNpcs
{
    public class GetAvailableNpcsCommandHandler : IRequestHandler<GetAvailableNpcsCommandRequest, GetAvailableNpcsCommandResponse>
    {
        private readonly INpcRepository npcRepository;

        public GetAvailableNpcsCommandHandler(INpcRepository npcRepository)
        {
            this.npcRepository = npcRepository;
        }

        public async Task<GetAvailableNpcsCommandResponse> Handle(GetAvailableNpcsCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<NpcDto> npcs = await npcRepository.GetAvailableNpcsByUserIdAsync(request.UserId);

            return new GetAvailableNpcsCommandResponse()
            {
                Npcs = npcs,
            };
        }
    }
}
