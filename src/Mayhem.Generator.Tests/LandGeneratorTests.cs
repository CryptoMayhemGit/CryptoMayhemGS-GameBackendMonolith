using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Generator.Tests.Base;
using Mayhem.Land.Bl.Dtos;
using Mayhem.Land.Bl.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Generator.Tests
{
    public class LandGeneratorTests : UnitTestBase
    {
        private ILandGeneratorService landGeneratorService;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            landGeneratorService = GetService<ILandGeneratorService>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task ReadLands_WhenLandsReaded_ThenGetThem_Test()
        {
            string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/lands.txt";
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(GenerateLands()));

                List<ImportLandDto> lands = await landGeneratorService.ReadLandsAsync();

                lands.Should().HaveCount(4);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Test]
        public async Task ReadAndSaveLands_WhenLandsSaved_ThenGetThem_Test()
        {
            string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/lands.txt";
            try
            {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(GenerateLands()));

                List<ImportLandDto> lands = await landGeneratorService.ReadLandsAsync();
                await landGeneratorService.SaveLandsAsync(lands);
                List<Dal.Tables.Nfts.Land> landsDb = await mayhemDataContext.Lands.ToListAsync();

                landsDb.Should().HaveCount(4);
                Assert.IsTrue(landsDb.All(x => x.LandInstanceId == 1));
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private static List<ImportLandDto> GenerateLands()
        {
            return new List<ImportLandDto>()
            {
                new ImportLandDto()
                {
                    Type = LandsType.Biome1,
                    X = 1,
                    Y = 1,
                },
                new ImportLandDto()
                {
                    Type = LandsType.Biome1,
                    X = 2,
                    Y = 1,
                },
                new ImportLandDto()
                {
                    Type = LandsType.Biome1,
                    X = 1,
                    Y = 2,
                },
                new ImportLandDto()
                {
                    Type = LandsType.Biome1,
                    X = 2,
                    Y = 2,
                }
            };
        }
    }
}
