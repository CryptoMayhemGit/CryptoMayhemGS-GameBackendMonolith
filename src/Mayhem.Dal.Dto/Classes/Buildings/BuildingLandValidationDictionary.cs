using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Dal.Dto.Classes.Buildings
{
    public static class BuildingLandValidationDictionary
    {
        public static Dictionary<LandsType, List<BuildingsType>> BuildingsPerSlot => new()
        {
            {
                LandsType.Swamp,
                new List<BuildingsType>()
                {
                    BuildingsType.Lumbermill,
                }
            },
            {
                LandsType.Forest,
                new List<BuildingsType>()
                {
                    BuildingsType.OreMine,
                }
            },
            {
                LandsType.Mountain,
                new List<BuildingsType>()
                {
                    BuildingsType.Slaughterhouse,
                    BuildingsType.Guardhouse,
                }
            },
            {
                LandsType.Desert,
                new List<BuildingsType>()
                {
                    BuildingsType.Farm,
                }
            },
            {
                LandsType.Field,
                new List<BuildingsType>()
                {
                    BuildingsType.MechanicalWorkshop,
                    BuildingsType.DroneFactory,
                    BuildingsType.CombatWorkshop,
                }
            },
            {
                LandsType.Biome1,
                new List<BuildingsType>()
                {
                }
            },
            {
                LandsType.Water,
                new List<BuildingsType>()
                {
                }
            }
        };
    }
}
