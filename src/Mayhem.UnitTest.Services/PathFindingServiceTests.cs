using FluentAssertions;
using Mayhem.Common.Services.PathFindingService.Dtos;
using Mayhem.Common.Services.PathFindingService.Enums;
using Mayhem.Common.Services.PathFindingService.Implementations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Mayhem.UnitTest.Services
{
    public class PathFindingServiceTests
    {
        private PathFindingService pathFindingService;

        private static List<PathFindingServiceTestsDto> cases => new()
        {
            new PathFindingServiceTestsDto(8, new PathLand(1, 1), new PathLand(6, 6), new List<PathLand>() { new PathLand(1, 0), new PathLand(0, 7), new PathLand(0, 6), new PathLand(7, 6), new PathLand(6, 6) }),
            new PathFindingServiceTestsDto(8, new PathLand(3, 2), new PathLand(4, 5), new List<PathLand>() { new PathLand(3, 3), new PathLand(4, 4), new PathLand(4, 5) }),
            new PathFindingServiceTestsDto(8, new PathLand(4, 6), new PathLand(2, 2), new List<PathLand>() { new PathLand(3, 5), new PathLand(3, 4), new PathLand(2, 3), new PathLand(2, 2) }),
            new PathFindingServiceTestsDto(8, new PathLand(1, 4), new PathLand(6, 4), new List<PathLand>() { new PathLand(0, 4), new PathLand(7, 4), new PathLand(6, 4) }),
            new PathFindingServiceTestsDto(8, new PathLand(3, 1), new PathLand(4, 6), new List<PathLand>() { new PathLand(3, 0), new PathLand(3, 7), new PathLand(4, 6) })
        };

        private static List<PathFindingServiceTestsDto> casesMapSize => new()
        {
            new PathFindingServiceTestsDto(10, new PathLand(2, 2), new PathLand(6, 6)),
            new PathFindingServiceTestsDto(20, new PathLand(2, 2), new PathLand(10, 10)),
            new PathFindingServiceTestsDto(50, new PathLand(2, 2), new PathLand(25, 25)),
            new PathFindingServiceTestsDto(75, new PathLand(2, 2), new PathLand(38, 38)),
            new PathFindingServiceTestsDto(100, new PathLand(2, 2), new PathLand(50, 50)),
            new PathFindingServiceTestsDto(150, new PathLand(2, 2), new PathLand(75, 75)),
            new PathFindingServiceTestsDto(200, new PathLand(2, 2), new PathLand(100, 100)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(125, 125)),
            new PathFindingServiceTestsDto(300, new PathLand(2, 2), new PathLand(150, 150)),
            new PathFindingServiceTestsDto(317, new PathLand(2, 2), new PathLand(180, 180)),
            new PathFindingServiceTestsDto(350, new PathLand(2, 2), new PathLand(200, 200)),
            new PathFindingServiceTestsDto(400, new PathLand(2, 2), new PathLand(200, 200)),
        };

        private static List<PathFindingServiceTestsDto> casesDistanceSize => new()
        {
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(6, 6)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(10, 10)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(25, 25)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(38, 38)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(50, 50)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(75, 75)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(100, 100)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(125, 125)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(150, 150)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(180, 180)),
            new PathFindingServiceTestsDto(250, new PathLand(2, 2), new PathLand(200, 200)),
        };

        [SetUp]
        public void SetUp()
        {
            pathFindingService = new PathFindingService();
        }

        [Test]
        public void GetPath_WhenPathExist_ThenGetIt_Test()
        {
            List<PathLand> result = pathFindingService.Calculate(PrepareGrid(true), new PathLand(1, 1), new PathLand(1, 8)).ToList();

            result.Count.Should().Be(30);

            result[0].X.Should().Be(2);
            result[0].Y.Should().Be(1);

            result[1].X.Should().Be(3);
            result[1].Y.Should().Be(1);

            result[2].X.Should().Be(4);
            result[2].Y.Should().Be(1);

            result[5].X.Should().Be(7);
            result[5].Y.Should().Be(1);

            result[29].X.Should().Be(1);
            result[29].Y.Should().Be(8);
        }

        [TestCaseSource(nameof(casesDistanceSize))]
        public void CalculateDifferentDistanceCases_Test(PathFindingServiceTestsDto @case)
        {
            List<PathLand> path = pathFindingService.Calculate(@case.Map, @case.Start, @case.Finish).ToList();

            path.Should().NotBeNull();
        }

        [TestCaseSource(nameof(casesMapSize))]
        public void CalculateDifferentMapCases_Test(PathFindingServiceTestsDto @case)
        {
            List<PathLand> path = pathFindingService.Calculate(@case.Map, @case.Start, @case.Finish).ToList();

            path.Should().NotBeNull();
        }

        [TestCaseSource(nameof(cases))]
        public void CalculateDifferentMapSize_Test(PathFindingServiceTestsDto @case)
        {
            List<PathLand> path = pathFindingService.Calculate(@case.Map, @case.Start, @case.Finish).ToList();

            for (int i = 0; i < path.Count; i++)
            {
                path[i].Should().Be(@case.Path[i]);
            }

            path.Should().NotBeNull();
        }

        [Test]
        public void GetPath_WhenPathNotExist_ThenGetEmpty_Test()
        {
            IEnumerable<PathLand> result = pathFindingService.Calculate(PrepareGrid(false), new PathLand(1, 1), new PathLand(1, 8));

            result.Should().BeNull();
        }

        private static int[,] PrepareGrid(bool pathPossible)
        {
            int[,] grid = new int[10, 10];

            grid[1, 1] = (int)PathFindingLandsType.PATH;
            grid[2, 1] = (int)PathFindingLandsType.PATH;
            grid[3, 1] = (int)PathFindingLandsType.PATH;
            grid[4, 1] = (int)PathFindingLandsType.PATH;
            grid[5, 1] = (int)PathFindingLandsType.PATH;
            grid[6, 1] = (int)PathFindingLandsType.PATH;
            grid[7, 1] = (int)PathFindingLandsType.PATH;
            grid[8, 1] = (int)PathFindingLandsType.PATH;

            grid[8, 2] = (int)PathFindingLandsType.PATH;

            grid[1, 3] = (int)PathFindingLandsType.PATH;
            grid[2, 3] = (int)PathFindingLandsType.PATH;
            grid[3, 3] = (int)PathFindingLandsType.PATH;
            grid[4, 3] = (int)PathFindingLandsType.PATH;
            grid[5, 3] = (int)PathFindingLandsType.PATH;
            grid[6, 3] = (int)PathFindingLandsType.PATH;
            grid[7, 3] = (int)PathFindingLandsType.PATH;
            grid[8, 3] = (int)PathFindingLandsType.PATH;
            
            grid[1, 4] = (int)PathFindingLandsType.PATH;

            grid[1, 5] = (int)PathFindingLandsType.PATH;
            grid[2, 5] = (int)PathFindingLandsType.PATH;
            grid[3, 5] = (int)PathFindingLandsType.PATH;
            grid[4, 5] = (int)PathFindingLandsType.PATH;
            grid[5, 5] = (int)PathFindingLandsType.PATH;
            grid[6, 5] = (int)PathFindingLandsType.PATH;
            grid[7, 5] = (int)PathFindingLandsType.PATH;
            grid[8, 5] = (int)PathFindingLandsType.PATH;

            grid[7, 6] = (int)PathFindingLandsType.PATH;
            grid[7, 7] = (int)PathFindingLandsType.PATH;

            grid[1, 8] = (int)PathFindingLandsType.PATH;
            grid[2, 8] = (int)PathFindingLandsType.PATH;
            grid[3, 8] = (int)PathFindingLandsType.PATH;
            grid[4, 8] = (int)PathFindingLandsType.PATH;
            grid[5, 8] = (int)PathFindingLandsType.PATH;
            grid[6, 8] = (int)PathFindingLandsType.PATH;
            grid[7, 8] = (int)PathFindingLandsType.PATH;
            grid[8, 8] = (int)PathFindingLandsType.PATH;

            if (!pathPossible)
            {
                grid[7, 8] = (int)PathFindingLandsType.WALL;
            }

            return grid;
        }
    }

    public class PathFindingServiceTestsDto
    {
        public PathLand Start { get; set; }
        public PathLand Finish { get; set; }
        public List<PathLand> Path { get; set; }

        public int[,] Map { get; set; }

        public PathFindingServiceTestsDto(int mapSize, PathLand start, PathLand finish, List<PathLand> path)
        {
            Start = start;
            Finish = finish;
            Path = path;
            CreateMap(mapSize);
        }

        public PathFindingServiceTestsDto(int mapSize, PathLand start, PathLand finish)
        {
            Start = start;
            Finish = finish;
            CreateMap(mapSize);
        }

        private void CreateMap(int mapSize)
        {
            Map = new int[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Map[j, i] = 1;
                }
            }
        }
    }
}
