using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetUnavailableItems
{
    public class GetUnavailableItemsCommandResponse
    {
        public IEnumerable<ItemDto> Items { get; set; }
    }
}
