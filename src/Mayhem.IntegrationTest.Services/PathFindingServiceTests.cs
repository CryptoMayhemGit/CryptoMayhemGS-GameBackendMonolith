using FluentAssertions;
using Mayhem.ApplicationSetup;
using Mayhem.Common.Services.Extensions;
using Mayhem.Common.Services.PathFindingService.Dtos;
using Mayhem.Common.Services.PathFindingService.Enums;
using Mayhem.Common.Services.PathFindingService.Interfaces;
using Mayhem.Configuration.Classes;
using Mayhem.Configuration.Extensions;
using Mayhem.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Mayhem.IntegrationTest.Services
{
    public class PathFindingServiceTests
    {
        private IPathFindingService pathFindingService;
        private IMayhemConfiguration configuration;
        private int[,] lands;

        [OneTimeSetUp]
        public void Setup()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(x => new ConfigurationBuilder().Build());
            IMayhemConfigurationService mayhemConfiguration = serviceCollection.AddMayhemConfigurationService(Environment.GetEnvironmentVariable(EnviromentVariables.MayhemAzureAppConfigurationConnecitonString), Environment.GetEnvironmentVariable(EnviromentVariables.MayhemConfigurationType));
            serviceCollection.AddMayhemContext(mayhemConfiguration);
            serviceCollection.AddCommonServices();
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            pathFindingService = serviceProvider.GetService<IPathFindingService>();
            configuration = serviceProvider.GetService<IMayhemConfiguration>();

            List<ImportLandTestDto> readedLands = JsonConvert.DeserializeObject<List<ImportLandTestDto>>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Source", "lands.txt")));

            lands = CreateGridFromLands(readedLands, configuration.CommonConfiguration.PlanetSize);
        }

        [Test]
        public void CalculatePathForSmallMap_Test()
        {
            IEnumerable<PathLand> result = pathFindingService.Calculate(lands, new PathLand(1, 1), new PathLand(50, 50));
            result.Should().NotBeNull();
        }

        [Test]
        public void CalculatePathForMediumMap_Test()
        {
            IEnumerable<PathLand> result = pathFindingService.Calculate(lands, new PathLand(1, 1), new PathLand(100, 100));
            result.Should().NotBeNull();
        }

        [Test]
        public void CalculatePathForHugeMap_Test()
        {
            IEnumerable<PathLand> result = pathFindingService.Calculate(lands, new PathLand(1, 1), new PathLand(200, 200));
            result.Should().NotBeNull();
        }

        [Test]
        public void CalculatePathForLargeMap_Test()
        {
            IEnumerable<PathLand> result = pathFindingService.Calculate(lands, new PathLand(1, 1), new PathLand(300, 300));
            result.Should().NotBeNull();
        }

        private int[,] CreateGridFromLands(List<ImportLandTestDto> lands, int arraySize)
        {
            int[,] planetLandArray = new int[arraySize, arraySize];
            for (int i = 0; i < lands.Count; i++)
            {
                planetLandArray[lands[i].X, lands[i].Y] = (int)PathFindingLandsType.PATH;
            }

            return planetLandArray;
        }
    }

    class ImportLandTestDto
    {
        public int Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
