using FluentAssertions;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.UnitTest.Base;
using Mayhen.Bl.Services.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;

namespace Mayhem.UnitTest.Services
{
    public class GuildImprovementValidationServiceTests : UnitTestBase
    {
        private IGuildImprovementValidationService guildImprovementValidationService;

        [OneTimeSetUp]
        public void Setup()
        {
            guildImprovementValidationService = GetService<IGuildImprovementValidationService>();
        }

        [TestCaseSource(nameof(GoodCases))]
        public void ValidateGoodCases_Tests(GuildImprovementValidationServiceTestCase @case)
        {
            bool result = guildImprovementValidationService.ValidateImprovement(@case.Level, @case.GuildBuildingsType, @case.GuildImprovements);

            result.Should().BeTrue();
        }

        [TestCaseSource(nameof(WrongCases))]
        public void ValidateWrongCases_Tests(GuildImprovementValidationServiceTestCase @case)
        {
            bool result = guildImprovementValidationService.ValidateImprovement(@case.Level, @case.GuildBuildingsType, @case.GuildImprovements);

            result.Should().BeFalse();
        }

        private static IEnumerable<GuildImprovementValidationServiceTestCase> GoodCases => new List<GuildImprovementValidationServiceTestCase>
                {
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 1,
                        GuildBuildingsType = GuildBuildingsType.AdriaCorporationHeadquarters,
                        GuildImprovements = new List<GuildImprovementsType>(),
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.AdriaCorporationHeadquarters,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.RegenerativeMeal,
                            GuildImprovementsType.Flashlight,
                            GuildImprovementsType.Motivator,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.AdriaCorporationHeadquarters,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.SupportPackage,
                            GuildImprovementsType.NeuralConditioning,
                            GuildImprovementsType.SIControlled,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 8,
                        GuildBuildingsType = GuildBuildingsType.AdriaCorporationHeadquarters,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.RegenerativeMeal,
                            GuildImprovementsType.Flashlight,
                            GuildImprovementsType.Motivator,
                             GuildImprovementsType.SupportPackage,
                            GuildImprovementsType.NeuralConditioning,
                            GuildImprovementsType.SIControlled,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.ExplorationBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.TerrainScanning,
                            GuildImprovementsType.AerialReconnaissance,
                            GuildImprovementsType.SoilSampling,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.ExplorationBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.MolecularAnalysis,
                            GuildImprovementsType.StatisticalAnalysis,
                            GuildImprovementsType.SatelliteReconnaissance,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 4,
                        GuildBuildingsType = GuildBuildingsType.ExplorationBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.TerrainScanning,
                            GuildImprovementsType.AerialReconnaissance,
                            GuildImprovementsType.SoilSampling,
                            GuildImprovementsType.MolecularAnalysis,
                            GuildImprovementsType.StatisticalAnalysis,
                            GuildImprovementsType.SatelliteReconnaissance,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.MechBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.PatternLibrary,
                            GuildImprovementsType.SheetMetalPressingPlant,
                            GuildImprovementsType.AssemblyLine,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.MechBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.WasteManagement,
                            GuildImprovementsType.ProductionMatrix,
                            GuildImprovementsType.ImprovedAssemblyLine,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 23,
                        GuildBuildingsType = GuildBuildingsType.MechBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.PatternLibrary,
                            GuildImprovementsType.SheetMetalPressingPlant,
                            GuildImprovementsType.AssemblyLine,
                            GuildImprovementsType.WasteManagement,
                            GuildImprovementsType.ProductionMatrix,
                            GuildImprovementsType.ImprovedAssemblyLine,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.FightBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.ImprovedEnergyProcessing,
                            GuildImprovementsType.WeaponReinforcement,
                            GuildImprovementsType.StrongerInternalStructure,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.FightBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.MechaniumI,
                            GuildImprovementsType.MechaniumII,
                            GuildImprovementsType.MechaniumIII,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 31,
                        GuildBuildingsType = GuildBuildingsType.FightBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.ImprovedEnergyProcessing,
                            GuildImprovementsType.WeaponReinforcement,
                            GuildImprovementsType.StrongerInternalStructure,
                            GuildImprovementsType.MechaniumI,
                            GuildImprovementsType.MechaniumII,
                            GuildImprovementsType.MechaniumIII,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.TransportBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.LargerWheels,
                            GuildImprovementsType.ImprovedTransmission,
                            GuildImprovementsType.PowerfulEngine,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.TransportBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.AdditionalDrive,
                            GuildImprovementsType.ImprovedFuelMixture,
                            GuildImprovementsType.LogisticSupport,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 13,
                        GuildBuildingsType = GuildBuildingsType.TransportBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.LargerWheels,
                            GuildImprovementsType.ImprovedTransmission,
                            GuildImprovementsType.PowerfulEngine,
                            GuildImprovementsType.AdditionalDrive,
                            GuildImprovementsType.ImprovedFuelMixture,
                            GuildImprovementsType.LogisticSupport,
                        },
                    },
                };

        private static IEnumerable<GuildImprovementValidationServiceTestCase> WrongCases => new List<GuildImprovementValidationServiceTestCase>()
                {
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.AdriaCorporationHeadquarters,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.RegenerativeMeal,
                            GuildImprovementsType.Motivator,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.AdriaCorporationHeadquarters,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.Flashlight,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 5,
                        GuildBuildingsType = GuildBuildingsType.AdriaCorporationHeadquarters,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.NeuralConditioning,
                            GuildImprovementsType.SheetMetalPressingPlant,
                            GuildImprovementsType.SupportPackage,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.ExplorationBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.TerrainScanning,
                            GuildImprovementsType.SoilSampling,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.ExplorationBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.StatisticalAnalysis,
                            GuildImprovementsType.AerialReconnaissance,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 4,
                        GuildBuildingsType = GuildBuildingsType.ExplorationBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.MolecularAnalysis,
                            GuildImprovementsType.StatisticalAnalysis,
                            GuildImprovementsType.SatelliteReconnaissance,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.MechBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.PatternLibrary,
                            GuildImprovementsType.SheetMetalPressingPlant,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.MechBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.WasteManagement,
                            GuildImprovementsType.ProductionMatrix,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 4,
                        GuildBuildingsType = GuildBuildingsType.MechBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                        },
                    },
                     new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.FightBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.ImprovedEnergyProcessing,
                            GuildImprovementsType.LargerWheels,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.FightBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.MechaniumII,
                            GuildImprovementsType.StrongerInternalStructure,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 6,
                        GuildBuildingsType = GuildBuildingsType.FightBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.MechaniumII,
                            GuildImprovementsType.MechaniumIII,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.TransportBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.LargerWheels,
                            GuildImprovementsType.PowerfulEngine,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 3,
                        GuildBuildingsType = GuildBuildingsType.TransportBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.ImprovedFuelMixture,
                            GuildImprovementsType.LogisticSupport,
                        },
                    },
                    new GuildImprovementValidationServiceTestCase()
                    {
                        Level = 2,
                        GuildBuildingsType = GuildBuildingsType.TransportBoard,
                        GuildImprovements = new List<GuildImprovementsType>()
                        {
                            GuildImprovementsType.ImprovedTransmission,
                            GuildImprovementsType.ImprovedFuelMixture,
                        },
                    },
                };
    }

    public class GuildImprovementValidationServiceTestCase
    {
        public int Level { get; set; }
        public GuildBuildingsType GuildBuildingsType { get; set; }
        public IEnumerable<GuildImprovementsType> GuildImprovements { get; set; }
    }
}
