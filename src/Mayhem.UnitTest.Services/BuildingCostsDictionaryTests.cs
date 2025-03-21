using FluentAssertions;
using Mayhem.Dal.Dto.Classes.Buildings;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Mayhem.UnitTest.Services
{
    public class BuildingCostsDictionaryTests
    {
        [TestCaseSource(nameof(Cases))]
        public void TestSomeCases_WhenCaseHasGoodValues_ThenValidateTrue_Test(BuildingCostsDictionaryTestsDto @case)
        {
            @case.Validate.Should().BeTrue();
        }

        private static List<BuildingCostsDictionaryTestsDto> Cases()
        {
            return new()
            {
                new BuildingCostsDictionaryTestsDto(4, BuildingsType.Lumbermill, lightWoodCost: 517, ironOreCost: 439, titaniumCost: 348, mechaniumCost: 272),
                new BuildingCostsDictionaryTestsDto(8, BuildingsType.Lumbermill, lightWoodCost: 1641, ironOreCost: 1308, titaniumCost: 1079, mechaniumCost: 848),
                new BuildingCostsDictionaryTestsDto(23, BuildingsType.Lumbermill, lightWoodCost: 25263, ironOreCost: 16358, titaniumCost: 13796, mechaniumCost: 10135),
                new BuildingCostsDictionaryTestsDto(41, BuildingsType.Lumbermill, lightWoodCost: 149275, ironOreCost: 86772, titaniumCost: 71824, mechaniumCost: 49756),

                new BuildingCostsDictionaryTestsDto(4, BuildingsType.OreMine, lightWoodCost: 407, ironOreCost: 434, titaniumCost: 424, mechaniumCost: 311),
                new BuildingCostsDictionaryTestsDto(20, BuildingsType.OreMine, lightWoodCost: 16173, ironOreCost: 11117, titaniumCost: 9800, mechaniumCost: 7202),
                new BuildingCostsDictionaryTestsDto(27, BuildingsType.OreMine, lightWoodCost: 40222, ironOreCost: 25672, titaniumCost: 22113, mechaniumCost: 15874),
                new BuildingCostsDictionaryTestsDto(46, BuildingsType.OreMine, lightWoodCost: 213026, ironOreCost: 122068, titaniumCost: 101476, mechaniumCost: 69300),

                new BuildingCostsDictionaryTestsDto(4, BuildingsType.MechanicalWorkshop, lightWoodCost: 464, heavyWoodCost: 467, titaniumCost: 420, mechaniumCost: 344),
                new BuildingCostsDictionaryTestsDto(9, BuildingsType.MechanicalWorkshop, lightWoodCost: 1985, heavyWoodCost: 1711, titaniumCost: 1529, mechaniumCost: 1234),
                new BuildingCostsDictionaryTestsDto(16, BuildingsType.MechanicalWorkshop, lightWoodCost: 8649, heavyWoodCost: 6299, titaniumCost: 5517, mechaniumCost: 4257),
                new BuildingCostsDictionaryTestsDto(39, BuildingsType.MechanicalWorkshop, lightWoodCost: 127124, heavyWoodCost: 75129, titaniumCost: 62783, mechaniumCost: 43932),

                new BuildingCostsDictionaryTestsDto(4, BuildingsType.DroneFactory, ironOreCost: 554, titaniumCost: 520, mechaniumCost: 368),
                new BuildingCostsDictionaryTestsDto(16, BuildingsType.DroneFactory, ironOreCost: 6645, titaniumCost: 5920, mechaniumCost: 4353),
                new BuildingCostsDictionaryTestsDto(19, BuildingsType.DroneFactory, ironOreCost: 10250, titaniumCost: 9025, mechaniumCost: 6592),
                new BuildingCostsDictionaryTestsDto(44, BuildingsType.DroneFactory, ironOreCost: 108261, titaniumCost: 90200, mechaniumCost: 61779),

                new BuildingCostsDictionaryTestsDto(4, BuildingsType.CombatWorkshop, ironOreCost: 496, heavyWoodCost: 501, titaniumCost: 424, mechaniumCost: 339),
                new BuildingCostsDictionaryTestsDto(26, BuildingsType.CombatWorkshop, ironOreCost: 23463, heavyWoodCost: 23494, titaniumCost: 19916, mechaniumCost: 14530),
                new BuildingCostsDictionaryTestsDto(39, BuildingsType.CombatWorkshop, ironOreCost: 75409, heavyWoodCost: 75456, titaniumCost: 62829, mechaniumCost: 43885),
                new BuildingCostsDictionaryTestsDto(47, BuildingsType.CombatWorkshop, ironOreCost: 130883, heavyWoodCost: 130940, titaniumCost: 108053, mechaniumCost: 73973),

                new BuildingCostsDictionaryTestsDto(4, BuildingsType.Farm, cereralCost: 508, lightWoodCost: 583, meatCost: 432, mechaniumCost: 344),
                new BuildingCostsDictionaryTestsDto(18, BuildingsType.Farm, cereralCost: 5833, lightWoodCost: 9050, meatCost: 4816, mechaniumCost: 5665),
                new BuildingCostsDictionaryTestsDto(23, BuildingsType.Farm, cereralCost: 10224, lightWoodCost: 17186, meatCost: 8312, mechaniumCost: 10549),
                new BuildingCostsDictionaryTestsDto(33, BuildingsType.Farm, cereralCost: 25032, lightWoodCost: 47040, meatCost: 19821, mechaniumCost: 27710),

                new BuildingCostsDictionaryTestsDto(4, BuildingsType.Slaughterhouse, cereralCost: 446, lightWoodCost: 439, heavyWoodCost: 472, mechaniumCost: 373),
                new BuildingCostsDictionaryTestsDto(9, BuildingsType.Slaughterhouse, cereralCost: 1410, lightWoodCost: 1646, heavyWoodCost: 1721, mechaniumCost: 1298),
                new BuildingCostsDictionaryTestsDto(31, BuildingsType.Slaughterhouse, cereralCost: 20849, lightWoodCost: 38236, heavyWoodCost: 38497, mechaniumCost: 23588),
                new BuildingCostsDictionaryTestsDto(46, BuildingsType.Slaughterhouse, cereralCost: 59337, lightWoodCost: 122123, heavyWoodCost: 122509, mechaniumCost: 70018),

                new BuildingCostsDictionaryTestsDto(4, BuildingsType.Guardhouse, heavyWoodCost: 463, titaniumCost: 410, ironOreCost: 458, mechaniumCost: 387),
                new BuildingCostsDictionaryTestsDto(20, BuildingsType.Guardhouse, heavyWoodCost: 11261, titaniumCost: 9728, ironOreCost: 11237, mechaniumCost: 7586),
                new BuildingCostsDictionaryTestsDto(29, BuildingsType.Guardhouse, heavyWoodCost: 31715, titaniumCost: 26895, ironOreCost: 31680, mechaniumCost: 19818),
                new BuildingCostsDictionaryTestsDto(39, BuildingsType.Guardhouse, heavyWoodCost: 75082, titaniumCost: 62689, ironOreCost: 75035, mechaniumCost: 44353),
            };
        }

        public class BuildingCostsDictionaryTestsDto
        {
            public int Level;
            public BuildingsType Type;

            public int? LightWoodCost;
            public int? HeavyWoodCost;
            public int? IronOreCost;
            public int? TitaniumCost;
            public int? CereralCost;
            public int? MeatCost;
            public int? MechaniumCost;

            public BuildingCostsDictionaryTestsDto(int level, BuildingsType type, int? lightWoodCost = null, int? heavyWoodCost = null, int? ironOreCost = null, int? titaniumCost = null, int? cereralCost = null, int? meatCost = null, int? mechaniumCost = null)
            {
                Level = level;
                Type = type;
                LightWoodCost = lightWoodCost;
                HeavyWoodCost = heavyWoodCost;
                IronOreCost = ironOreCost;
                TitaniumCost = titaniumCost;
                CereralCost = cereralCost;
                MeatCost = meatCost;
                MechaniumCost = mechaniumCost;
            }

            private Dictionary<ResourcesType, int> Costs => BuildingCostsDictionary.GetBuildingCosts(Type, Level);

            public bool Validate => new List<bool>()
                {
                    !LightWoodCost.HasValue || Costs[ResourcesType.LightWood] == LightWoodCost.Value,
                    !HeavyWoodCost.HasValue || Costs[ResourcesType.HeavyWood] == HeavyWoodCost.Value,
                    !IronOreCost.HasValue || Costs[ResourcesType.IronOre] == IronOreCost.Value,
                    !TitaniumCost.HasValue || Costs[ResourcesType.TitaniumOre] == TitaniumCost.Value,
                    !CereralCost.HasValue || Costs[ResourcesType.Cereal] == CereralCost.Value,
                    !MeatCost.HasValue || Costs[ResourcesType.Meat] == MeatCost.Value,
                    !MechaniumCost.HasValue || Costs[ResourcesType.Mechanium] == MechaniumCost.Value,
                }.All(x => x);
        }
    }
}
