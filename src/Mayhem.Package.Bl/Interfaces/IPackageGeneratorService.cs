using System.Collections.Generic;

namespace Mayhem.Package.Bl.Interfaces
{
    /// <summary>
    /// Package Generator Service
    /// </summary>
    public interface IPackageGeneratorService
    {
        /// <summary>
        /// Generates the packages.
        /// </summary>
        /// <param name="packageAmount">The package amount.</param>
        /// <returns></returns>
        IEnumerable<Dal.Dto.Classes.Generator.Package> GeneratePackages(int packageAmount);
    }
}
