using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Messages;

namespace Mayhem.Dal.Dto.Classes.GuildBuildings
{
    public static class GuildBuildingBonusDictionary
    {
        public static double GetGuildBuildingBonusValues(GuildBuildingsType guildBuildingsType, int level)
        {
            return guildBuildingsType switch
            {
                GuildBuildingsType.AdriaCorporationHeadquarters => level switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    int l when l > 1 => 0.1,
                    _ => 0,
                },
                GuildBuildingsType.ExplorationBoard => level switch
                {
                    1 => 10,
                    2 => 20,
                    3 => 30,
                    int l when l > 1 => 0.1,
                    _ => 0,
                },
                GuildBuildingsType.MechBoard => level switch
                {
                    1 => 5,
                    2 => 10,
                    3 => 15,
                    int l when l > 1 => 0.1,
                    _ => 0,
                },
                GuildBuildingsType.FightBoard => level switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    int l when l > 1 => 0.1,
                    _ => 0,
                },
                GuildBuildingsType.TransportBoard => level switch
                {
                    1 => 10,
                    2 => 15,
                    3 => 20,
                    int l when l > 1 => 0.1,
                    _ => 0,
                },
                _ => 0,
            };
        }

        public static GuildBuildingBonusesType GetBuildingBonusesType(GuildBuildingsType guildBuildingType)
        {
            return guildBuildingType switch
            {
                GuildBuildingsType.AdriaCorporationHeadquarters => GuildBuildingBonusesType.All,
                GuildBuildingsType.ExplorationBoard => GuildBuildingBonusesType.DiscoveryDetection,
                GuildBuildingsType.MechBoard => GuildBuildingBonusesType.MechConstruction,
                GuildBuildingsType.FightBoard => GuildBuildingBonusesType.MechAttack,
                GuildBuildingsType.TransportBoard => GuildBuildingBonusesType.MoveSpeed,
                _ => throw ExceptionMessages.EnumOutOfRangeException(nameof(GuildBuildingBonusesType)),
            };
        }
    }
}
