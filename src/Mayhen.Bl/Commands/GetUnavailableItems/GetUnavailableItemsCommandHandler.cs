using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetUnavailableItems
{
    public class GetUnavailableItemsCommandHandler : IRequestHandler<GetUnavailableItemsCommandRequest, GetUnavailableItemsCommandResponse>
    {
        public readonly IItemRepository itemRepository;

        public GetUnavailableItemsCommandHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<GetUnavailableItemsCommandResponse> Handle(GetUnavailableItemsCommandRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<ItemDto> items = await itemRepository.GetUnavailableItemsByUserIdAsync(request.UserId);

            return new GetUnavailableItemsCommandResponse()
            {
                Items = items,
            };
        }
    }
}
