using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Services.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;

namespace Mayhem.UnitTest.Services
{
    public class ImprovementValidationServiceTests : UnitTestBase
    {
        private IImprovementValidationService improvementValidationService;

        [OneTimeSetUp]
        public void Setup()
        {
            improvementValidationService = GetService<IImprovementValidationService>();
        }

        [TestCaseSource(nameof(GoodCases))]
        public void ValidateGoodCases_Tests(ImprovementValidationServiceTestCase @case)
        {
            bool result = improvementValidationService.ValidateImprovement(@case.Level, @case.BuildingsType, @case.Improvements);

            result.Should().BeTrue();
        }

        [TestCaseSource(nameof(WrongCases))]
        public void ValidateWrongCases_Tests(ImprovementValidationServiceTestCase @case)
        {
            bool result = improvementValidationService.ValidateImprovement(@case.Level, @case.BuildingsType, @case.Improvements);

            result.Should().BeFalse();
        }

        private static IEnumerable<ImprovementValidationServiceTestCase> GoodCases => new List<ImprovementValidationServiceTestCase>()
                {
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 1,
                        BuildingsType = BuildingsType.Lumbermill,
                        Improvements = new List<ImprovementsType>(),
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Lumbermill,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.ReinforcedChainsawMotor,
                            ImprovementsType.ImprovedGear,
                            ImprovementsType.HardenedSawChain,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Lumbermill,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.TreeScanner,
                            ImprovementsType.EnergyCells,
                            ImprovementsType.LaserBlade,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 8,
                        BuildingsType = BuildingsType.Lumbermill,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.ReinforcedChainsawMotor,
                            ImprovementsType.ImprovedGear,
                            ImprovementsType.HardenedSawChain,
                            ImprovementsType.TreeScanner,
                            ImprovementsType.EnergyCells,
                            ImprovementsType.LaserBlade,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.OreMine,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.TitaniumPickaxe,
                            ImprovementsType.DiamondHeadDrillBit,
                            ImprovementsType.BasicMiningCombine,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.OreMine,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.DeepExcavationsTechnology,
                            ImprovementsType.MiningShaftsVentilation,
                            ImprovementsType.OreElectrolyticRefining,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 4,
                        BuildingsType = BuildingsType.OreMine,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.TitaniumPickaxe,
                            ImprovementsType.DiamondHeadDrillBit,
                            ImprovementsType.BasicMiningCombine,
                            ImprovementsType.DeepExcavationsTechnology,
                            ImprovementsType.MiningShaftsVentilation,
                            ImprovementsType.OreElectrolyticRefining,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.MechanicalWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.QuickDryingCement,
                            ImprovementsType.ReinforcedFoundations,
                            ImprovementsType.ModularMaterials,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.MechanicalWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.SpecialistBrigade,
                            ImprovementsType.PrefabricatedElements,
                            ImprovementsType.ConstructionRobot,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 23,
                        BuildingsType = BuildingsType.MechanicalWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.QuickDryingCement,
                            ImprovementsType.ReinforcedFoundations,
                            ImprovementsType.ModularMaterials,
                            ImprovementsType.SpecialistBrigade,
                            ImprovementsType.PrefabricatedElements,
                            ImprovementsType.ConstructionRobot,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.CombatWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.FullShellMissiles,
                            ImprovementsType.WeaponModificationDrives,
                            ImprovementsType.HighEnergyPowder,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.CombatWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.BattlefieldRadar,
                            ImprovementsType.LaserGuidance,
                            ImprovementsType.AssistedAI,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 31,
                        BuildingsType = BuildingsType.CombatWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.FullShellMissiles,
                            ImprovementsType.WeaponModificationDrives,
                            ImprovementsType.HighEnergyPowder,
                            ImprovementsType.BattlefieldRadar,
                            ImprovementsType.LaserGuidance,
                            ImprovementsType.AssistedAI,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.Seedling,
                            ImprovementsType.SelectedSoil,
                            ImprovementsType.Fertilizers,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.PlantGrafting,
                            ImprovementsType.ProtectiveMeasures,
                            ImprovementsType.GeneticModification,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 13,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.Seedling,
                            ImprovementsType.SelectedSoil,
                            ImprovementsType.Fertilizers,
                            ImprovementsType.PlantGrafting,
                            ImprovementsType.ProtectiveMeasures,
                            ImprovementsType.GeneticModification,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                           ImprovementsType.Seedling,
                            ImprovementsType.SelectedSoil,
                            ImprovementsType.Fertilizers,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.PlantGrafting,
                            ImprovementsType.ProtectiveMeasures,
                            ImprovementsType.GeneticModification,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 9,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.Seedling,
                            ImprovementsType.SelectedSoil,
                            ImprovementsType.Fertilizers,
                            ImprovementsType.PlantGrafting,
                            ImprovementsType.ProtectiveMeasures,
                            ImprovementsType.GeneticModification,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Slaughterhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.HealthyFeed,
                            ImprovementsType.NaturalSelectionControl,
                            ImprovementsType.VeterinaryCare,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Slaughterhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.SustainableBreeding,
                            ImprovementsType.IncreasedAnimalWelfare,
                            ImprovementsType.GeneticSupport
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 9,
                        BuildingsType = BuildingsType.Slaughterhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.HealthyFeed,
                            ImprovementsType.NaturalSelectionControl,
                            ImprovementsType.VeterinaryCare,
                            ImprovementsType.SustainableBreeding,
                            ImprovementsType.IncreasedAnimalWelfare,
                            ImprovementsType.GeneticSupport
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Guardhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.SituationalRecognition,
                            ImprovementsType.ObservationDrone,
                            ImprovementsType.InfraredObservation,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Guardhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.FireControlDrone,
                            ImprovementsType.ReinforcedAmmunition,
                            ImprovementsType.ImprovedFireControlComputer
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 9,
                        BuildingsType = BuildingsType.Guardhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.SituationalRecognition,
                            ImprovementsType.ObservationDrone,
                            ImprovementsType.InfraredObservation,
                            ImprovementsType.FireControlDrone,
                            ImprovementsType.ReinforcedAmmunition,
                            ImprovementsType.ImprovedFireControlComputer
                        },
                    },
                };

        private static IEnumerable<ImprovementValidationServiceTestCase> WrongCases => new List<ImprovementValidationServiceTestCase>()
                {
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Lumbermill,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.ReinforcedChainsawMotor,
                            ImprovementsType.ImprovedGear,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Lumbermill,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.TreeScanner,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 5,
                        BuildingsType = BuildingsType.Lumbermill,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.ReinforcedChainsawMotor,
                            ImprovementsType.ImprovedGear,
                            ImprovementsType.TreeScanner,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.OreMine,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.TitaniumPickaxe,
                            ImprovementsType.DiamondHeadDrillBit,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.OreMine,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.DeepExcavationsTechnology,
                            ImprovementsType.OreElectrolyticRefining,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 4,
                        BuildingsType = BuildingsType.OreMine,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.DeepExcavationsTechnology,
                            ImprovementsType.MiningShaftsVentilation,
                            ImprovementsType.OreElectrolyticRefining,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.MechanicalWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.QuickDryingCement,
                            ImprovementsType.ModularMaterials,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.MechanicalWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.PrefabricatedElements,
                            ImprovementsType.ConstructionRobot,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 4,
                        BuildingsType = BuildingsType.MechanicalWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                        },
                    },
                     new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.CombatWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.FullShellMissiles,
                            ImprovementsType.HighEnergyPowder,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.CombatWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.BattlefieldRadar,
                            ImprovementsType.LaserGuidance,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 6,
                        BuildingsType = BuildingsType.CombatWorkshop,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.BattlefieldRadar,
                            ImprovementsType.LaserGuidance,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.SelectedSoil,
                            ImprovementsType.Fertilizers,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Farm,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.PlantGrafting,
                            ImprovementsType.GeneticModification,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Slaughterhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.HealthyFeed,
                            ImprovementsType.VeterinaryCare,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Slaughterhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.SustainableBreeding,
                            ImprovementsType.IncreasedAnimalWelfare,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        BuildingsType = BuildingsType.Guardhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.SituationalRecognition,
                            ImprovementsType.ObservationDrone,
                        },
                    },
                    new ImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        BuildingsType = BuildingsType.Guardhouse,
                        Improvements = new List<ImprovementsType>()
                        {
                            ImprovementsType.FireControlDrone,
                            ImprovementsType.ReinforcedAmmunition,
                        },
                    },
                };
    }

    public class ImprovementValidationServiceTestCase
    {
        public int Level { get; set; }
        public BuildingsType BuildingsType { get; set; }
        public IEnumerable<ImprovementsType> Improvements { get; set; }
    }
}
