using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetAvailableItems
{
    public class GetAvailableItemsCommandResponse
    {
        public IEnumerable<ItemDto> Items { get; set; }
    }
}
