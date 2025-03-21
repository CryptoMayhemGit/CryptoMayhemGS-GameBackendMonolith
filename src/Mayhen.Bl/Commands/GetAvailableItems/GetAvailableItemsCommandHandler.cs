using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetAvailableItems
{
    public class GetAvailableItemsCommandHandler : IRequestHandler<GetAvailableItemsCommandRequest, GetAvailableItemsCommandResponse>
    {
        public readonly IItemRepository itemRepository;

        public GetAvailableItemsCommandHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<GetAvailableItemsCommandResponse> Handle(GetAvailableItemsCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<ItemDto> items = await itemRepository.GetAvailableItemsByUserIdAsync(request.UserId);

            return new GetAvailableItemsCommandResponse()
            {
                Items = items,
            };
        }
    }
}
