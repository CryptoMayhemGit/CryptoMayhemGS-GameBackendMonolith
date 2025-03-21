using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.AssignItemToNpc
{
    public class AssignItemToNpcCommandHandler : IRequestHandler<AssignItemToNpcCommandRequest, AssignItemToNpcCommandResponse>
    {
        private readonly IItemRepository itemRepository;

        public AssignItemToNpcCommandHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<AssignItemToNpcCommandResponse> Handle(AssignItemToNpcCommandRequest request, CancellationToken cancellationToken)
        {
            bool status = await itemRepository.AssignItemToNpcAsync(request.NpcId, request.ItemId, request.UserId);

            return new AssignItemToNpcCommandResponse()
            {
                Status = status,
            };
        }
    }
}
