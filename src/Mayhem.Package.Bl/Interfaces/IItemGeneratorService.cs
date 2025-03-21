using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhem.Package.Bl.Interfaces
{
    /// <summary>
    /// Item Generator Service
    /// </summary>
    public interface IItemGeneratorService
    {
        /// <summary>
        /// Generates the items.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItemDto> GenerateItems();
    }
}
