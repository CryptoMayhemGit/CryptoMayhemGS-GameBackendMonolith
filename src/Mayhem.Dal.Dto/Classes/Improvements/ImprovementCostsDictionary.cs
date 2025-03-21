using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Classes.Improvements
{
    public static class ImprovementCostsDictionary
    {
        public static Dictionary<ResourcesType, int> GetImprovementCosts(ImprovementsType type)
        {
            return type switch
            {
                ImprovementsType.ReinforcedChainsawMotor => new()
                {
                    { ResourcesType.LightWood, 12 },
                    { ResourcesType.TitaniumOre, 3 },
                    { ResourcesType.IronOre, 6 },
                    { ResourcesType.Mechanium, 1 },
                },
                ImprovementsType.ImprovedGear => new()
                {
                    { ResourcesType.LightWood, 20 },
                    { ResourcesType.TitaniumOre, 7 },
                    { ResourcesType.IronOre, 11 },
                    { ResourcesType.Mechanium, 3 },
                },
                ImprovementsType.HardenedSawChain => new()
                {
                    { ResourcesType.LightWood, 34 },
                    { ResourcesType.TitaniumOre, 14 },
                    { ResourcesType.IronOre, 22 },
                    { ResourcesType.Mechanium, 9 },
                },
                ImprovementsType.TreeScanner => new()
                {
                    { ResourcesType.LightWood, 37 },
                    { ResourcesType.TitaniumOre, 18 },
                    { ResourcesType.IronOre, 27 },
                    { ResourcesType.Mechanium, 12 },
                },
                ImprovementsType.EnergyCells => new()
                {
                    { ResourcesType.LightWood, 47 },
                    { ResourcesType.TitaniumOre, 25 },
                    { ResourcesType.IronOre, 39 },
                    { ResourcesType.Mechanium, 18 },
                },
                ImprovementsType.LaserBlade => new()
                {
                    { ResourcesType.LightWood, 57 },
                    { ResourcesType.TitaniumOre, 36 },
                    { ResourcesType.IronOre, 52 },
                    { ResourcesType.Mechanium, 25 },
                },
                ImprovementsType.TitaniumPickaxe => new()
                {
                    { ResourcesType.LightWood, 7 },
                    { ResourcesType.TitaniumOre, 7 },
                    { ResourcesType.IronOre, 4 },
                    { ResourcesType.Mechanium, 2 },
                },
                ImprovementsType.DiamondHeadDrillBit => new()
                {
                    { ResourcesType.LightWood, 11 },
                    { ResourcesType.TitaniumOre, 16 },
                    { ResourcesType.IronOre, 8 },
                    { ResourcesType.Mechanium, 5 },
                },
                ImprovementsType.BasicMiningCombine => new()
                {
                    { ResourcesType.LightWood, 20 },
                    { ResourcesType.TitaniumOre, 29 },
                    { ResourcesType.IronOre, 17 },
                    { ResourcesType.Mechanium, 10 },
                },
                ImprovementsType.DeepExcavationsTechnology => new()
                {
                    { ResourcesType.LightWood, 24 },
                    { ResourcesType.TitaniumOre, 33 },
                    { ResourcesType.IronOre, 21 },
                    { ResourcesType.Mechanium, 13 },
                },
                ImprovementsType.MiningShaftsVentilation => new()
                {
                    { ResourcesType.LightWood, 35 },
                    { ResourcesType.TitaniumOre, 40 },
                    { ResourcesType.IronOre, 33 },
                    { ResourcesType.Mechanium, 19 },
                },
                ImprovementsType.OreElectrolyticRefining => new()
                {
                    { ResourcesType.LightWood, 44 },
                    { ResourcesType.TitaniumOre, 50 },
                    { ResourcesType.IronOre, 42 },
                    { ResourcesType.Mechanium, 26 },
                },
                ImprovementsType.QuickDryingCement => new()
                {
                    { ResourcesType.LightWood, 5 },
                    { ResourcesType.HeavyWood, 9 },
                    { ResourcesType.TitaniumOre, 4 },
                    { ResourcesType.Mechanium, 1 },
                },
                ImprovementsType.ReinforcedFoundations => new()
                {
                    { ResourcesType.LightWood, 9 },
                    { ResourcesType.HeavyWood, 20 },
                    { ResourcesType.TitaniumOre, 10 },
                    { ResourcesType.Mechanium, 3 },
                },
                ImprovementsType.ModularMaterials => new()
                {
                    { ResourcesType.LightWood, 17 },
                    { ResourcesType.HeavyWood, 31 },
                    { ResourcesType.TitaniumOre, 19 },
                    { ResourcesType.Mechanium, 7 },
                },
                ImprovementsType.SpecialistBrigade => new()
                {
                    { ResourcesType.LightWood, 24 },
                    { ResourcesType.HeavyWood, 37 },
                    { ResourcesType.TitaniumOre, 25 },
                    { ResourcesType.Mechanium, 10 },
                },
                ImprovementsType.PrefabricatedElements => new()
                {
                    { ResourcesType.LightWood, 37 },
                    { ResourcesType.HeavyWood, 51 },
                    { ResourcesType.TitaniumOre, 31 },
                    { ResourcesType.Mechanium, 16 },
                },
                ImprovementsType.ConstructionRobot => new()
                {
                    { ResourcesType.LightWood, 50 },
                    { ResourcesType.HeavyWood, 62 },
                    { ResourcesType.TitaniumOre, 41 },
                    { ResourcesType.Mechanium, 22 },
                },
                ImprovementsType.ReinforcedRotors => new()
                {
                    { ResourcesType.IronOre, 10 },
                    { ResourcesType.TitaniumOre, 10 },
                    { ResourcesType.Mechanium, 5 },
                },
                ImprovementsType.TitaniumHull => new()
                {
                    { ResourcesType.IronOre, 21 },
                    { ResourcesType.TitaniumOre, 18 },
                    { ResourcesType.Mechanium, 9 },
                },
                ImprovementsType.EnlargedScraper => new()
                {
                    { ResourcesType.IronOre, 37 },
                    { ResourcesType.TitaniumOre, 29 },
                    { ResourcesType.Mechanium, 16 },
                },
                ImprovementsType.AdditionalCovers => new()
                {
                    { ResourcesType.IronOre, 46 },
                    { ResourcesType.TitaniumOre, 36 },
                    { ResourcesType.Mechanium, 22 },
                },
                ImprovementsType.PrecisePositioning => new()
                {
                    { ResourcesType.IronOre, 52 },
                    { ResourcesType.TitaniumOre, 44 },
                    { ResourcesType.Mechanium, 30 },
                },
                ImprovementsType.AdditionalTankForMechanium => new()
                {
                    { ResourcesType.IronOre, 64 },
                    { ResourcesType.TitaniumOre, 57 },
                    { ResourcesType.Mechanium, 41 },
                },
                ImprovementsType.FullShellMissiles => new()
                {
                    { ResourcesType.IronOre, 8 },
                    { ResourcesType.HeavyWood, 3 },
                    { ResourcesType.TitaniumOre, 5 },
                    { ResourcesType.Mechanium, 2 },
                },
                ImprovementsType.WeaponModificationDrives => new()
                {
                    { ResourcesType.IronOre, 17 },
                    { ResourcesType.HeavyWood, 11 },
                    { ResourcesType.TitaniumOre, 10 },
                    { ResourcesType.Mechanium, 4 },
                },
                ImprovementsType.HighEnergyPowder => new()
                {
                    { ResourcesType.IronOre, 28 },
                    { ResourcesType.HeavyWood, 24 },
                    { ResourcesType.TitaniumOre, 19 },
                    { ResourcesType.Mechanium, 10 },
                },
                ImprovementsType.BattlefieldRadar => new()
                {
                    { ResourcesType.IronOre, 48 },
                    { ResourcesType.HeavyWood, 45 },
                    { ResourcesType.TitaniumOre, 41 },
                    { ResourcesType.Mechanium, 20 },
                },
                ImprovementsType.LaserGuidance => new()
                {
                    { ResourcesType.IronOre, 59 },
                    { ResourcesType.HeavyWood, 60 },
                    { ResourcesType.TitaniumOre, 49 },
                    { ResourcesType.Mechanium, 29 },
                },
                ImprovementsType.AssistedAI => new()
                {
                    { ResourcesType.IronOre, 68 },
                    { ResourcesType.HeavyWood, 69 },
                    { ResourcesType.TitaniumOre, 57 },
                    { ResourcesType.Mechanium, 38 },
                },
                ImprovementsType.Seedling => new()
                {
                    { ResourcesType.Cereal, 8 },
                    { ResourcesType.LightWood, 10 },
                    { ResourcesType.Meat, 2 },
                    { ResourcesType.Mechanium, 2 },
                },
                ImprovementsType.SelectedSoil => new()
                {
                    { ResourcesType.Cereal, 15 },
                    { ResourcesType.LightWood, 21 },
                    { ResourcesType.Meat, 9 },
                    { ResourcesType.Mechanium, 4 },
                },
                ImprovementsType.Fertilizers => new()
                {
                    { ResourcesType.Cereal, 32 },
                    { ResourcesType.LightWood, 38 },
                    { ResourcesType.Meat, 17 },
                    { ResourcesType.Mechanium, 10 },
                },
                ImprovementsType.PlantGrafting => new()
                {
                    { ResourcesType.Cereal, 55 },
                    { ResourcesType.LightWood, 62 },
                    { ResourcesType.Meat, 38 },
                    { ResourcesType.Mechanium, 19 },
                },
                ImprovementsType.ProtectiveMeasures => new()
                {
                    { ResourcesType.Cereal, 64 },
                    { ResourcesType.LightWood, 73 },
                    { ResourcesType.Meat, 50 },
                    { ResourcesType.Mechanium, 27 },
                },
                ImprovementsType.GeneticModification => new()
                {
                    { ResourcesType.Cereal, 73 },
                    { ResourcesType.LightWood, 83 },
                    { ResourcesType.Meat, 57 },
                    { ResourcesType.Mechanium, 36 },
                },
                ImprovementsType.HealthyFeed => new()
                {
                    { ResourcesType.Cereal, 10 },
                    { ResourcesType.LightWood, 5 },
                    { ResourcesType.HeavyWood, 5 },
                    { ResourcesType.Mechanium, 1 },
                },
                ImprovementsType.NaturalSelectionControl => new()
                {
                    { ResourcesType.Cereal, 17 },
                    { ResourcesType.LightWood, 12 },
                    { ResourcesType.HeavyWood, 11 },
                    { ResourcesType.Mechanium, 3 },
                },
                ImprovementsType.VeterinaryCare => new()
                {
                    { ResourcesType.Cereal, 27 },
                    { ResourcesType.LightWood, 24 },
                    { ResourcesType.HeavyWood, 19 },
                    { ResourcesType.Mechanium, 9 },
                },
                ImprovementsType.SustainableBreeding => new()
                {
                    { ResourcesType.Cereal, 47 },
                    { ResourcesType.LightWood, 39 },
                    { ResourcesType.HeavyWood, 40 },
                    { ResourcesType.Mechanium, 20 },
                },
                ImprovementsType.IncreasedAnimalWelfare => new()
                {
                    { ResourcesType.Cereal, 55 },
                    { ResourcesType.LightWood, 48 },
                    { ResourcesType.HeavyWood, 51 },
                    { ResourcesType.Mechanium, 29 },
                },
                ImprovementsType.GeneticSupport => new()
                {
                    { ResourcesType.Cereal, 67 },
                    { ResourcesType.LightWood, 57 },
                    { ResourcesType.HeavyWood, 58 },
                    { ResourcesType.Mechanium, 40 },
                },
                ImprovementsType.SituationalRecognition => new()
                {
                    { ResourcesType.HeavyWood, 4 },
                    { ResourcesType.TitaniumOre, 6 },
                    { ResourcesType.IronOre, 7 },
                    { ResourcesType.Mechanium, 2 },
                },
                ImprovementsType.ObservationDrone => new()
                {
                    { ResourcesType.HeavyWood, 11 },
                    { ResourcesType.TitaniumOre, 12 },
                    { ResourcesType.IronOre, 17 },
                    { ResourcesType.Mechanium, 4 },
                },
                ImprovementsType.InfraredObservation => new()
                {
                    { ResourcesType.HeavyWood, 22 },
                    { ResourcesType.TitaniumOre, 20 },
                    { ResourcesType.IronOre, 31 },
                    { ResourcesType.Mechanium, 11 },
                },
                ImprovementsType.FireControlDrone => new()
                {
                    { ResourcesType.HeavyWood, 36 },
                    { ResourcesType.TitaniumOre, 29 },
                    { ResourcesType.IronOre, 39 },
                    { ResourcesType.Mechanium, 21 },
                },
                ImprovementsType.ReinforcedAmmunition => new()
                {
                    { ResourcesType.HeavyWood, 45 },
                    { ResourcesType.TitaniumOre, 35 },
                    { ResourcesType.IronOre, 51 },
                    { ResourcesType.Mechanium, 30 },
                },
                ImprovementsType.ImprovedFireControlComputer => new()
                {
                    { ResourcesType.HeavyWood, 59 },
                    { ResourcesType.TitaniumOre, 46 },
                    { ResourcesType.IronOre, 64 },
                    { ResourcesType.Mechanium, 41 },
                },
                _ => null,
            };
        }
    }
}