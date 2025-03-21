using FluentAssertions;
using Mayhem.Dal.Dto.Classes.Generator;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class PackageRepositoryTests : UnitTestBase
    {
        private IMayhemDataContext mayhemDataContext;
        private IPackageRepository packageRepository;

        [OneTimeSetUp]
        public void SetUp()
        {
            mayhemDataContext = GetService<IMayhemDataContext>();
            packageRepository = GetService<IPackageRepository>();
        }

        [Test]
        public async Task GeneratePackages_WhenPackagesGenerated_ThenSaveThemAndGetResults_Tests()
        {
            await packageRepository.AddItemWithNpcAsync(GetPackages());

            List<Npc> npc = await mayhemDataContext.Npcs.ToListAsync();
            List<Item> items = await mayhemDataContext.Items.ToListAsync();

            npc.Should().HaveCount(4);
            items.Should().HaveCount(8);
        }

        private static List<Package> GetPackages()
        {
            return new List<Package>()
            {
                new Package()
                {
                    Npcs = new List<NpcDto>()
                    {
                        new NpcDto()
                        {
                            Name = "npc 1",
                        },
                        new NpcDto()
                        {
                            Name = "npc 2",
                        },
                        new NpcDto()
                        {
                            Name = "npc 3",
                        },
                        new NpcDto()
                        {
                            Name = "npc 4",
                        }
                    },
                    MatchingItems = new List<ItemDto>()
                    {
                        new ItemDto()
                        {
                            Name = "item 1",
                        },
                        new ItemDto()
                        {
                            Name = "item 2",
                        },
                        new ItemDto()
                        {
                            Name = "item 3",
                        },
                        new ItemDto()
                        {
                            Name = "item 4",
                        },
                    },
                    MismatchingItems = new List<ItemDto>()
                    {
                        new ItemDto()
                        {
                            Name = "item 5",
                        },
                        new ItemDto()
                        {
                            Name = "item 6",
                        },
                        new ItemDto()
                        {
                            Name = "item 7",
                        },
                        new ItemDto()
                        {
                            Name = "item 8",
                        },
                    },
                }
            };
        }
    }
}
