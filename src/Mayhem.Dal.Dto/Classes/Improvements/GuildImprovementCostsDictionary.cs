using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Classes.Improvements
{
    public static class GuildImprovementCostsDictionary
    {
        public static Dictionary<ResourcesType, int> GetGuildImprovementCosts(GuildImprovementsType guildImprovementsType)
        {
            return guildImprovementsType switch
            {
                GuildImprovementsType.RegenerativeMeal => new()
                {
                    { ResourcesType.HeavyWood, 55000 },
                    { ResourcesType.TitaniumOre, 35000 },
                    { ResourcesType.IronOre, 44000 },
                    { ResourcesType.Mechanium, 20000 },
                },
                GuildImprovementsType.Flashlight => new()
                {
                    { ResourcesType.HeavyWood, 67000 },
                    { ResourcesType.TitaniumOre, 44000 },
                    { ResourcesType.IronOre, 57000 },
                    { ResourcesType.Mechanium, 28000 },
                },
                GuildImprovementsType.Motivator => new()
                {
                    { ResourcesType.HeavyWood, 75000 },
                    { ResourcesType.TitaniumOre, 54000 },
                    { ResourcesType.IronOre, 69000 },
                    { ResourcesType.Mechanium, 36000 },
                },
                GuildImprovementsType.SupportPackage => new()
                {
                    { ResourcesType.HeavyWood, 99000 },
                    { ResourcesType.TitaniumOre, 68000 },
                    { ResourcesType.IronOre, 78000 },
                    { ResourcesType.Mechanium, 48000 },
                },
                GuildImprovementsType.NeuralConditioning => new()
                {
                    { ResourcesType.HeavyWood, 110000 },
                    { ResourcesType.TitaniumOre, 80000 },
                    { ResourcesType.IronOre, 88000 },
                    { ResourcesType.Mechanium, 55000 },
                },
                GuildImprovementsType.SIControlled => new()
                {
                    { ResourcesType.HeavyWood, 150000 },
                    { ResourcesType.TitaniumOre, 110000 },
                    { ResourcesType.IronOre, 120000 },
                    { ResourcesType.Mechanium, 75000 },
                },
                GuildImprovementsType.TerrainScanning => new()
                {
                    { ResourcesType.HeavyWood, 42000 },
                    { ResourcesType.TitaniumOre, 24000 },
                    { ResourcesType.IronOre, 30000 },
                    { ResourcesType.Mechanium, 13000 },
                },
                GuildImprovementsType.AerialReconnaissance => new()
                {
                    { ResourcesType.HeavyWood, 51000 },
                    { ResourcesType.TitaniumOre, 31000 },
                    { ResourcesType.IronOre, 42000 },
                    { ResourcesType.Mechanium, 19000 },
                },
                GuildImprovementsType.SoilSampling => new()
                {
                    { ResourcesType.HeavyWood, 61000 },
                    { ResourcesType.TitaniumOre, 39000 },
                    { ResourcesType.IronOre, 53000 },
                    { ResourcesType.Mechanium, 27000 },
                },
                GuildImprovementsType.MolecularAnalysis => new()
                {
                    { ResourcesType.HeavyWood, 78000 },
                    { ResourcesType.TitaniumOre, 53000 },
                    { ResourcesType.IronOre, 60000 },
                    { ResourcesType.Mechanium, 42000 },
                },
                GuildImprovementsType.StatisticalAnalysis => new()
                {
                    { ResourcesType.HeavyWood, 90000 },
                    { ResourcesType.TitaniumOre, 62000 },
                    { ResourcesType.IronOre, 69000 },
                    { ResourcesType.Mechanium, 50000 },
                },
                GuildImprovementsType.SatelliteReconnaissance => new()
                {
                    { ResourcesType.HeavyWood, 125000 },
                    { ResourcesType.TitaniumOre, 75000 },
                    { ResourcesType.IronOre, 85000 },
                    { ResourcesType.Mechanium, 75000 },
                },
                GuildImprovementsType.PatternLibrary => new()
                {
                    { ResourcesType.HeavyWood, 38000 },
                    { ResourcesType.TitaniumOre, 22000 },
                    { ResourcesType.IronOre, 26000 },
                    { ResourcesType.Mechanium, 11000 },
                },
                GuildImprovementsType.SheetMetalPressingPlant => new()
                {
                    { ResourcesType.HeavyWood, 45000 },
                    { ResourcesType.TitaniumOre, 29000 },
                    { ResourcesType.IronOre, 32000 },
                    { ResourcesType.Mechanium, 18000 },
                },
                GuildImprovementsType.AssemblyLine => new()
                {
                    { ResourcesType.HeavyWood, 57000 },
                    { ResourcesType.TitaniumOre, 38000 },
                    { ResourcesType.IronOre, 44000 },
                    { ResourcesType.Mechanium, 25000 },
                },
                GuildImprovementsType.WasteManagement => new()
                {
                    { ResourcesType.HeavyWood, 74000 },
                    { ResourcesType.TitaniumOre, 56000 },
                    { ResourcesType.IronOre, 52000 },
                    { ResourcesType.Mechanium, 39000 },
                },
                GuildImprovementsType.ProductionMatrix => new()
                {
                    { ResourcesType.HeavyWood, 87000 },
                    { ResourcesType.TitaniumOre, 65000 },
                    { ResourcesType.IronOre, 61000 },
                    { ResourcesType.Mechanium, 45000 },
                },
                GuildImprovementsType.ImprovedAssemblyLine => new()
                {
                    { ResourcesType.HeavyWood, 105000 },
                    { ResourcesType.TitaniumOre, 90000 },
                    { ResourcesType.IronOre, 80000 },
                    { ResourcesType.Mechanium, 70000 },
                },
                GuildImprovementsType.ImprovedEnergyProcessing => new()
                {
                    { ResourcesType.HeavyWood, 39000 },
                    { ResourcesType.TitaniumOre, 27000 },
                    { ResourcesType.IronOre, 31000 },
                    { ResourcesType.Mechanium, 14000 },
                },
                GuildImprovementsType.WeaponReinforcement => new()
                {
                    { ResourcesType.HeavyWood, 50000 },
                    { ResourcesType.TitaniumOre, 36000 },
                    { ResourcesType.IronOre, 39000 },
                    { ResourcesType.Mechanium, 22000 },
                },
                GuildImprovementsType.StrongerInternalStructure => new()
                {
                    { ResourcesType.HeavyWood, 62000 },
                    { ResourcesType.TitaniumOre, 45000 },
                    { ResourcesType.IronOre, 48000 },
                    { ResourcesType.Mechanium, 30000 },
                },
                GuildImprovementsType.MechaniumI => new()
                {
                    { ResourcesType.HeavyWood, 80000 },
                    { ResourcesType.TitaniumOre, 64000 },
                    { ResourcesType.IronOre, 56000 },
                    { ResourcesType.Mechanium, 47000 },
                },
                GuildImprovementsType.MechaniumII => new()
                {
                    { ResourcesType.HeavyWood, 95000 },
                    { ResourcesType.TitaniumOre, 72000 },
                    { ResourcesType.IronOre, 65000 },
                    { ResourcesType.Mechanium, 54000 },
                },
                GuildImprovementsType.MechaniumIII => new()
                {
                    { ResourcesType.HeavyWood, 135000 },
                    { ResourcesType.TitaniumOre, 100000 },
                    { ResourcesType.IronOre, 90000 },
                    { ResourcesType.Mechanium, 80000 },
                },
                GuildImprovementsType.LargerWheels => new()
                {
                    { ResourcesType.HeavyWood, 25000 },
                    { ResourcesType.TitaniumOre, 16000 },
                    { ResourcesType.IronOre, 19000 },
                    { ResourcesType.Mechanium, 9000 },
                },
                GuildImprovementsType.ImprovedTransmission => new()
                {
                    { ResourcesType.HeavyWood, 30000 },
                    { ResourcesType.TitaniumOre, 27000 },
                    { ResourcesType.IronOre, 24000 },
                    { ResourcesType.Mechanium, 14000 },
                },
                GuildImprovementsType.PowerfulEngine => new()
                {
                    { ResourcesType.HeavyWood, 38000 },
                    { ResourcesType.TitaniumOre, 35000 },
                    { ResourcesType.IronOre, 29000 },
                    { ResourcesType.Mechanium, 21000 },
                },
                GuildImprovementsType.AdditionalDrive => new()
                {
                    { ResourcesType.HeavyWood, 55000 },
                    { ResourcesType.TitaniumOre, 60000 },
                    { ResourcesType.IronOre, 41000 },
                    { ResourcesType.Mechanium, 36000 },
                },
                GuildImprovementsType.ImprovedFuelMixture => new()
                {
                    { ResourcesType.HeavyWood, 64000 },
                    { ResourcesType.TitaniumOre, 70000 },
                    { ResourcesType.IronOre, 49000 },
                    { ResourcesType.Mechanium, 44000 },
                },
                GuildImprovementsType.LogisticSupport => new()
                {
                    { ResourcesType.HeavyWood, 90000 },
                    { ResourcesType.TitaniumOre, 95000 },
                    { ResourcesType.IronOre, 75000 },
                    { ResourcesType.Mechanium, 75000 },
                },
                _ => new()
            };
        }
    }
}
