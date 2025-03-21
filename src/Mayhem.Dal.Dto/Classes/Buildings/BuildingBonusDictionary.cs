using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Messages;

namespace Mayhem.Dal.Dto.Classes.Buildings
{
    public static class BuildingBonusDictionary
    {
        public static double GetBuildingBonusValues(BuildingsType buildingType, int level)
        {
            return buildingType switch
            {
                BuildingsType.Lumbermill => level switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    int l when l > 3 => 3 + (l - 3) * 0.1,
                    _ => 0,
                },
                BuildingsType.OreMine => level switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    int l when l > 3 => 3 + (l - 3) * 0.1,
                    _ => 0,
                },
                BuildingsType.MechanicalWorkshop => level switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    int l when l > 3 => 3 + (l - 3) * 0.1,
                    _ => 0,
                },
                BuildingsType.DroneFactory => level switch
                {
                    1 => 0,
                    2 => 1,
                    3 => 2,
                    int l when l > 3 => 2 + (l - 3) * 0.1,
                    _ => 0,
                },
                BuildingsType.CombatWorkshop => level switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    int l when l > 3 => 3 + (l - 3) * 0.1,
                    _ => 0,
                },
                BuildingsType.Farm => level switch
                {
                    1 => 0,
                    2 => 10,
                    3 => 20,
                    int l when l > 3 => 20 + (l - 3),
                    _ => 0,
                },
                BuildingsType.Slaughterhouse => level switch
                {
                    1 => 0,
                    2 => 20,
                    3 => 10,
                    int l when l > 3 => 20 + (l - 3),
                    _ => 0,
                },
                BuildingsType.Guardhouse => level switch
                {
                    1 => 0,
                    2 => 10,
                    3 => 20,
                    int l when l > 3 => 20 + (l - 3),
                    _ => 0,
                },
                _ => 0,
            };
        }

        public static BuildingBonusesType GetBuildingBonusesType(BuildingsType buildingType)
        {
            return buildingType switch
            {
                BuildingsType.Lumbermill => BuildingBonusesType.Wood,
                BuildingsType.OreMine => BuildingBonusesType.Mining,
                BuildingsType.MechanicalWorkshop => BuildingBonusesType.Construction,
                BuildingsType.DroneFactory => BuildingBonusesType.MechaniumCollection,
                BuildingsType.CombatWorkshop => BuildingBonusesType.Attack,
                BuildingsType.Farm => BuildingBonusesType.Cereal,
                BuildingsType.Slaughterhouse => BuildingBonusesType.Meat,
                BuildingsType.Guardhouse => BuildingBonusesType.Attack,
                _ => throw ExceptionMessages.EnumOutOfRangeException(nameof(BuildingsType)),
            };
        }
    }
}
