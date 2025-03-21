using FluentAssertions;
using Mayhem.Dal.Dto.Classes.GuildBuildings;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Mayhem.UnitTest.Services
{
    public class GuildBuildingCostsDictionaryTests
    {
        [TestCaseSource(nameof(Cases))]
        public void TestSomeCases_WhenCaseHasGoodValues_ThenValidateTrue_Test(GuildBuildingCostsDictionaryTestsDto @case)
        {
            @case.Validate.Should().BeTrue();
        }

        private static List<GuildBuildingCostsDictionaryTestsDto> Cases()
        {
            return new()
            {
                new GuildBuildingCostsDictionaryTestsDto(4, GuildBuildingsType.AdriaCorporationHeadquarters, heavyWoodCost: 1200064, titaniumCost: 936069, ironOreCost: 1104056, mechaniumCost: 744056),
                new GuildBuildingCostsDictionaryTestsDto(8, GuildBuildingsType.AdriaCorporationHeadquarters, heavyWoodCost: 2400512, titaniumCost: 1872569, ironOreCost: 2208416, mechaniumCost: 1488416),
                new GuildBuildingCostsDictionaryTestsDto(23, GuildBuildingsType.AdriaCorporationHeadquarters, heavyWoodCost: 6912167, titaniumCost: 5396233, ironOreCost: 6356893, mechaniumCost: 4286893),
                new GuildBuildingCostsDictionaryTestsDto(41, GuildBuildingsType.AdriaCorporationHeadquarters, heavyWoodCost: 12368921, titaniumCost: 9676984, ironOreCost: 11363542, mechaniumCost: 7673542),

                new GuildBuildingCostsDictionaryTestsDto(4, GuildBuildingsType.ExplorationBoard, heavyWoodCost: 1056064, titaniumCost: 720069, ironOreCost: 816056, mechaniumCost: 624056),
                new GuildBuildingCostsDictionaryTestsDto(20, GuildBuildingsType.ExplorationBoard, heavyWoodCost: 5288000, titaniumCost: 3609293, ironOreCost: 4085930, mechaniumCost: 3125930),
                new GuildBuildingCostsDictionaryTestsDto(27, GuildBuildingsType.ExplorationBoard, heavyWoodCost: 7147683, titaniumCost: 4883210, ironOreCost: 5522157, mechaniumCost: 4226157),
                new GuildBuildingCostsDictionaryTestsDto(46, GuildBuildingsType.ExplorationBoard, heavyWoodCost: 12241336, titaniumCost: 8397873, ironOreCost: 9450374, mechaniumCost: 7242374),

                new GuildBuildingCostsDictionaryTestsDto(4, GuildBuildingsType.MechBoard, heavyWoodCost: 912064, titaniumCost: 840069, ironOreCost: 744056, mechaniumCost: 672056),
                new GuildBuildingCostsDictionaryTestsDto(9, GuildBuildingsType.MechBoard, heavyWoodCost: 2052729, titaniumCost: 1890814, ironOreCost: 1674586, mechaniumCost: 1512586),
                new GuildBuildingCostsDictionaryTestsDto(16, GuildBuildingsType.MechBoard, heavyWoodCost: 3652096, titaniumCost: 3364706, ironOreCost: 2979105, mechaniumCost: 2691105),
                new GuildBuildingCostsDictionaryTestsDto(39, GuildBuildingsType.MechBoard, heavyWoodCost: 8951319, titaniumCost: 8261244, ironOreCost: 7295124, mechaniumCost: 6593124),

                new GuildBuildingCostsDictionaryTestsDto(4, GuildBuildingsType.FightBoard, heavyWoodCost: 960064, titaniumCost: 840069, ironOreCost: 696056, mechaniumCost: 720056),
                new GuildBuildingCostsDictionaryTestsDto(16, GuildBuildingsType.FightBoard, heavyWoodCost: 3844096, titaniumCost: 3364706, ironOreCost: 2787105, mechaniumCost: 2883105),
                new GuildBuildingCostsDictionaryTestsDto(19, GuildBuildingsType.FightBoard, heavyWoodCost: 4566859, titaniumCost: 3997947, ironOreCost: 3311110, mechaniumCost: 3425110),
                new GuildBuildingCostsDictionaryTestsDto(44, GuildBuildingsType.FightBoard, heavyWoodCost: 10645184, titaniumCost: 9342928, ironOreCost: 7714347, mechaniumCost: 7978347),

                new GuildBuildingCostsDictionaryTestsDto(4, GuildBuildingsType.TransportBoard, heavyWoodCost: 792064, titaniumCost: 1008069, ironOreCost: 768056, mechaniumCost: 600056),
                new GuildBuildingCostsDictionaryTestsDto(26, GuildBuildingsType.TransportBoard, heavyWoodCost: 5165576, titaniumCost: 6572686, ironOreCost: 5004689, mechaniumCost: 3912689),
                new GuildBuildingCostsDictionaryTestsDto(39, GuildBuildingsType.TransportBoard, heavyWoodCost: 7781319, titaniumCost: 9899244, ironOreCost: 7529124, mechaniumCost: 5891124),
                new GuildBuildingCostsDictionaryTestsDto(47, GuildBuildingsType.TransportBoard, heavyWoodCost: 9409823, titaniumCost: 11969864, ironOreCost: 9094646, mechaniumCost: 7120646),
            };
        }
    }

    public class GuildBuildingCostsDictionaryTestsDto
    {
        public int Level;
        public GuildBuildingsType Type;

        public int? LightWoodCost;
        public int? HeavyWoodCost;
        public int? IronOreCost;
        public int? TitaniumCost;
        public int? CereralCost;
        public int? MeatCost;
        public int? MechaniumCost;

        public GuildBuildingCostsDictionaryTestsDto(int level, GuildBuildingsType type, int? lightWoodCost = null, int? heavyWoodCost = null, int? ironOreCost = null, int? titaniumCost = null, int? cereralCost = null, int? meatCost = null, int? mechaniumCost = null)
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

        private Dictionary<ResourcesType, int> Costs => GuildBuildingCostsDictionary.GetGuildBuildingCosts(Type, Level);

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
