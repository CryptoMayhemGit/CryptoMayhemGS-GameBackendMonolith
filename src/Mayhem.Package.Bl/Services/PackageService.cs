using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Package.Bl.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Package.Bl.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageGeneratorService packageGeneratorService;
        private readonly IPackageRepository packageRepository;

        public PackageService(
            IPackageGeneratorService packageGeneratorService,
            IPackageRepository packageRepository)
        {
            this.packageGeneratorService = packageGeneratorService;
            this.packageRepository = packageRepository;
        }

        public IEnumerable<Dal.Dto.Classes.Generator.Package> GenerateAndValidate(int packageAmount)
        {
            IEnumerable<Dal.Dto.Classes.Generator.Package> packages = packageGeneratorService.GeneratePackages(packageAmount);

            if (!ValidatePackages(packages))
            {
                throw new Exception("Couldn't generate packages");
            }

            return packages;
        }

        public async Task<bool> InsertNftsAsync(IEnumerable<Dal.Dto.Classes.Generator.Package> packages)
        {
            List<Dal.Dto.Classes.Generator.Package> packagesList = packages.ToList();

            StringBuilder sb = new();
            for (int i = 0; i < packagesList.Count; i++)
            {
                sb.AppendLine(packagesList[i].ToPackageString(i));
            }
            File.WriteAllText($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\packages.txt", sb.ToString());

            await packageRepository.AddItemWithNpcAsync(packagesList);

            return true;
        }

        private static bool ValidatePackages(IEnumerable<Dal.Dto.Classes.Generator.Package> packages)
        {
            foreach (Dal.Dto.Classes.Generator.Package package in packages)
            {
                if (package.Npcs.Count != 4)
                {
                    return false;
                }
                if (package.MismatchingItems.Count != 4)
                {
                    return false;
                }
                if (package.MatchingItems.Count != 4)
                {
                    return false;
                }
                if (package.Npcs.Where(x => x.IsAvatar).Count() > 1)
                {
                    return false;
                }
                if (package.Npcs.Select(x => x.NpcTypeId).GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList().Count > 0)
                {
                    return false;
                }
                if (package.Npcs.Where(x => x.IsAvatar).Count() > 1)
                {
                    return false;
                }
            }

            if (packages.SelectMany(x => x.Npcs.Where(x => x.IsAvatar)).Count() != 224)
            {
                return false;
            }
            if (packages.SelectMany(x => x.Npcs.Where(x => !x.IsAvatar)).Count() != 4784)
            {
                return false;
            }
            if (packages.SelectMany(x => x.MatchingItems.Union(x.MismatchingItems)).Count() != 10016)
            {
                return false;
            }

            return true;
        }
    }
}
