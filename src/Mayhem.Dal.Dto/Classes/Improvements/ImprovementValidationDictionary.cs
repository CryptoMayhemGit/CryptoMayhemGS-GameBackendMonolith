using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Classes.Improvements
{
    public static class ImprovementValidationDictionary
    {
        public static ICollection<ImprovementsType> Level2Lumbermill => new List<ImprovementsType>
        {
            ImprovementsType.ReinforcedChainsawMotor,
            ImprovementsType.ImprovedGear,
            ImprovementsType.HardenedSawChain,
        };

        public static ICollection<ImprovementsType> Level3Lumbermill => new List<ImprovementsType>
        {
            ImprovementsType.TreeScanner,
            ImprovementsType.EnergyCells,
            ImprovementsType.LaserBlade,
        };

        public static ICollection<ImprovementsType> Level2OreMine => new List<ImprovementsType>
        {
            ImprovementsType.TitaniumPickaxe,
            ImprovementsType.DiamondHeadDrillBit,
            ImprovementsType.BasicMiningCombine,
        };

        public static ICollection<ImprovementsType> Level3OreMine => new List<ImprovementsType>
        {
            ImprovementsType.DeepExcavationsTechnology,
            ImprovementsType.MiningShaftsVentilation,
            ImprovementsType.OreElectrolyticRefining,
        };

        public static ICollection<ImprovementsType> Level2MechanicalWorkshop => new List<ImprovementsType>
        {
            ImprovementsType.QuickDryingCement,
            ImprovementsType.ReinforcedFoundations,
            ImprovementsType.ModularMaterials,
        };

        public static ICollection<ImprovementsType> Level3MechanicalWorkshop => new List<ImprovementsType>
        {
            ImprovementsType.SpecialistBrigade,
            ImprovementsType.PrefabricatedElements,
            ImprovementsType.ConstructionRobot,
        };

        public static ICollection<ImprovementsType> Level2DroneFactory => new List<ImprovementsType>
        {
            ImprovementsType.ReinforcedRotors,
            ImprovementsType.TitaniumHull,
            ImprovementsType.EnlargedScraper,
        };

        public static ICollection<ImprovementsType> Level3DroneFactory => new List<ImprovementsType>
        {
            ImprovementsType.AdditionalCovers,
            ImprovementsType.PrecisePositioning,
            ImprovementsType.AdditionalTankForMechanium,
        };

        public static ICollection<ImprovementsType> Level2CombatWorkshop => new List<ImprovementsType>
        {
            ImprovementsType.FullShellMissiles,
            ImprovementsType.WeaponModificationDrives,
            ImprovementsType.HighEnergyPowder,
        };

        public static ICollection<ImprovementsType> Level3CombatWorkshop => new List<ImprovementsType>
        {
            ImprovementsType.BattlefieldRadar,
            ImprovementsType.LaserGuidance,
            ImprovementsType.AssistedAI,
        };

        public static ICollection<ImprovementsType> Level2Farm => new List<ImprovementsType>
        {
            ImprovementsType.Seedling,
            ImprovementsType.SelectedSoil,
            ImprovementsType.Fertilizers,
        };

        public static ICollection<ImprovementsType> Level3Farm => new List<ImprovementsType>
        {
            ImprovementsType.PlantGrafting,
            ImprovementsType.ProtectiveMeasures,
            ImprovementsType.GeneticModification,
        };

        public static ICollection<ImprovementsType> Level2Slaughterhouse => new List<ImprovementsType>
        {
            ImprovementsType.HealthyFeed,
            ImprovementsType.NaturalSelectionControl,
            ImprovementsType.VeterinaryCare,
        };

        public static ICollection<ImprovementsType> Level3Slaughterhouse => new List<ImprovementsType>
        {
            ImprovementsType.SustainableBreeding,
            ImprovementsType.IncreasedAnimalWelfare,
            ImprovementsType.GeneticSupport,
        };

        public static ICollection<ImprovementsType> Level2Guardhouse => new List<ImprovementsType>
        {
            ImprovementsType.SituationalRecognition,
            ImprovementsType.ObservationDrone,
            ImprovementsType.InfraredObservation,
        };

        public static ICollection<ImprovementsType> Level3Guardhouse => new List<ImprovementsType>
        {
            ImprovementsType.FireControlDrone,
            ImprovementsType.ReinforcedAmmunition,
            ImprovementsType.ImprovedFireControlComputer,
        };
    }
}