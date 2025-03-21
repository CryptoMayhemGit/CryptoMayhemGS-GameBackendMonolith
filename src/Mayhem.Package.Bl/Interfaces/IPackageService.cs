using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Package.Bl.Interfaces
{
    /// <summary>
    /// Package Service
    /// </summary>
    public interface IPackageService
    {
        /// <summary>
        /// Generates the and validate.
        /// </summary>
        /// <param name="packageAmount">The package amount.</param>
        /// <returns></returns>
        IEnumerable<Dal.Dto.Classes.Generator.Package> GenerateAndValidate(int packageAmount);
        /// <summary>
        /// Inserts the NFTS asynchronous.
        /// </summary>
        /// <param name="packages">The packages.</param>
        /// <returns></returns>
        Task<bool> InsertNftsAsync(IEnumerable<Dal.Dto.Classes.Generator.Package> packages);
    }
}
