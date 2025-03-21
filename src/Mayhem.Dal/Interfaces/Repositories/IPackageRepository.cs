using Mayhem.Dal.Dto.Classes.Generator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Package Repository
    /// </summary>
    public interface IPackageRepository
    {
        /// <summary>
        /// Adds the item with NPC asynchronous.
        /// </summary>
        /// <param name="packagesList">The packages list.</param>
        /// <returns></returns>
        Task AddItemWithNpcAsync(List<Package> packagesList);
    }
}
