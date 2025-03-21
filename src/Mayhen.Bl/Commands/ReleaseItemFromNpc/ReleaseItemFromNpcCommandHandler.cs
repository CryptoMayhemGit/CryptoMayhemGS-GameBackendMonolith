using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.ReleaseItemFromNpc
{
    public class ReleaseItemFromNpcCommandHandler : IRequestHandler<ReleaseItemFromNpcCommandRequest, ReleaseItemFromNpcCommandResponse>
    {
        private readonly IItemRepository itemRepository;

        public ReleaseItemFromNpcCommandHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<ReleaseItemFromNpcCommandResponse> Handle(ReleaseItemFromNpcCommandRequest request, CancellationToken cancellationToken)
        {
            bool status = await itemRepository.ReleaseItemFromNpcAsync(request.ItemId, request.UserId);

            return new ReleaseItemFromNpcCommandResponse()
            {
                Status = status,
            };
        }
    }
}
