using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhen.Bl.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.ChangeNpcHealthState
{
    public class ChangeNpcHealthStateCommandHandler : IRequestHandler<ChangeNpcHealthStateCommandRequest, ChangeNpcHealthStateCommandResponse>
    {
        private readonly IDamageService damageService;
        private readonly INpcRepository npcRepository;

        public ChangeNpcHealthStateCommandHandler(IDamageService damageService, INpcRepository npcRepository)
        {
            this.damageService = damageService;
            this.npcRepository = npcRepository;
        }

        public async Task<ChangeNpcHealthStateCommandResponse> Handle(ChangeNpcHealthStateCommandRequest request, CancellationToken cancellationToken)
        {
            NpcDto npc = await npcRepository.GetUserNpcByIdWithAttributesAsync(request.NpcId, request.UserId);

            if (npc.NpcHealthStateId == request.NpcHealthState)
            {
                return new ChangeNpcHealthStateCommandResponse()
                {
                    Result = true,
                };
            }

            List<AttributeDto> attributes = npc.Attributes.ToList();

            damageService.SetAttributesBasedOnHealth(attributes, npc.NpcHealthStateId, request.NpcHealthState);

            bool result = await npcRepository.UpdateNpcHealthWithAttributesAsync(request.NpcId, attributes, request.NpcHealthState);

            return new ChangeNpcHealthStateCommandResponse()
            {
                Result = result,
            };
        }
    }
}
