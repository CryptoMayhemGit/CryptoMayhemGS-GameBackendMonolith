using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Classes.Improvements
{
    public static class GuildImprovementValidationDictionary
    {
        public static ICollection<GuildImprovementsType> Level2AdriaCorporationHeadquarters => new List<GuildImprovementsType>
        {
            GuildImprovementsType.RegenerativeMeal,
            GuildImprovementsType.Flashlight,
            GuildImprovementsType.Motivator,
        };

        public static ICollection<GuildImprovementsType> Level3AdriaCorporationHeadquarters => new List<GuildImprovementsType>
        {
            GuildImprovementsType.SupportPackage,
            GuildImprovementsType.NeuralConditioning,
            GuildImprovementsType.SIControlled,
        };

        public static ICollection<GuildImprovementsType> Level2ExplorationBoard => new List<GuildImprovementsType>
        {
            GuildImprovementsType.TerrainScanning,
            GuildImprovementsType.AerialReconnaissance,
            GuildImprovementsType.SoilSampling,
        };

        public static ICollection<GuildImprovementsType> Level3ExplorationBoard => new List<GuildImprovementsType>
        {
            GuildImprovementsType.MolecularAnalysis,
            GuildImprovementsType.StatisticalAnalysis,
            GuildImprovementsType.SatelliteReconnaissance,
        };

        public static ICollection<GuildImprovementsType> Level2MechManagement => new List<GuildImprovementsType>
        {
            GuildImprovementsType.PatternLibrary,
            GuildImprovementsType.SheetMetalPressingPlant,
            GuildImprovementsType.AssemblyLine,
        };

        public static ICollection<GuildImprovementsType> Level3MechManagement => new List<GuildImprovementsType>
        {
            GuildImprovementsType.WasteManagement,
            GuildImprovementsType.ProductionMatrix,
            GuildImprovementsType.ImprovedAssemblyLine,
        };

        public static ICollection<GuildImprovementsType> Level2FightBoard => new List<GuildImprovementsType>
        {
            GuildImprovementsType.ImprovedEnergyProcessing,
            GuildImprovementsType.WeaponReinforcement,
            GuildImprovementsType.StrongerInternalStructure,
        };

        public static ICollection<GuildImprovementsType> Level3FightBoard => new List<GuildImprovementsType>
        {
            GuildImprovementsType.MechaniumI,
            GuildImprovementsType.MechaniumII,
            GuildImprovementsType.MechaniumIII,
        };

        public static ICollection<GuildImprovementsType> Level2TransportAuthority => new List<GuildImprovementsType>
        {
            GuildImprovementsType.LargerWheels,
            GuildImprovementsType.ImprovedTransmission,
            GuildImprovementsType.PowerfulEngine,
        };

        public static ICollection<GuildImprovementsType> Level3TransportAuthority => new List<GuildImprovementsType>
        {
            GuildImprovementsType.AdditionalDrive,
            GuildImprovementsType.ImprovedFuelMixture,
            GuildImprovementsType.LogisticSupport,
        };
    }
}
