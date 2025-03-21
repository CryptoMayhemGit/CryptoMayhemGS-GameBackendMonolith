using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mayhem.Dal.Dto.Classes.Attributes
{
    public static class AttributeBonusDictionary
    {
        public static IEnumerable<AttributesType> GetAttributeTypesByItemBonusType(ItemBonusesType itemBonusType)
        {
            return itemBonusType switch
            {
                ItemBonusesType.Wood => new List<AttributesType>() { AttributesType.HeavyWoodProduction, AttributesType.LightWoodProduction },
                ItemBonusesType.Mining => new List<AttributesType>() { AttributesType.IronOreProduction, AttributesType.TitaniumProduction },
                ItemBonusesType.Meat => new List<AttributesType>() { AttributesType.MeatProduction },
                ItemBonusesType.Cereal => new List<AttributesType>() { AttributesType.CerealProduction },
                ItemBonusesType.Attack => new List<AttributesType>() { AttributesType.Attack },
                ItemBonusesType.Healing => new List<AttributesType>() { AttributesType.Healing },
                ItemBonusesType.MoveSpeed => new List<AttributesType>() { AttributesType.MoveSpeed },
                ItemBonusesType.Discovery => new List<AttributesType>() { AttributesType.Discovery },
                ItemBonusesType.Repair => new List<AttributesType>() { AttributesType.Repair },
                ItemBonusesType.Construction => new List<AttributesType>() { AttributesType.Construction },
                ItemBonusesType.Detection => new List<AttributesType>() { AttributesType.Detection },
                ItemBonusesType.MechProduction => new List<AttributesType>() { AttributesType.MechProduction },
                _ => throw ExceptionMessages.EnumOutOfRangeException(nameof(ItemBonusesType)),
            };
        }

        public static IEnumerable<AttributesType> GetAttributeTypesByBuildingBonusType(BuildingBonusesType buildingBonusType)
        {
            return buildingBonusType switch
            {
                BuildingBonusesType.Wood => new List<AttributesType>() { AttributesType.HeavyWoodProduction, AttributesType.LightWoodProduction },
                BuildingBonusesType.Mining => new List<AttributesType>() { AttributesType.IronOreProduction, AttributesType.TitaniumProduction },
                BuildingBonusesType.Construction => new List<AttributesType>() { AttributesType.Construction },
                BuildingBonusesType.MechaniumCollection => new List<AttributesType>(),
                BuildingBonusesType.Attack => new List<AttributesType>() { AttributesType.Attack },
                BuildingBonusesType.Cereal => new List<AttributesType>() { AttributesType.CerealProduction },
                BuildingBonusesType.Meat => new List<AttributesType>() { AttributesType.MeatProduction },
                _ => throw ExceptionMessages.EnumOutOfRangeException(nameof(BuildingBonusesType)),
            };
        }

        public static IEnumerable<AttributesType> GetAttributeTypesByGuildBuildingType(GuildBuildingsType guildBuildingType)
        {
            return guildBuildingType switch
            {
                GuildBuildingsType.AdriaCorporationHeadquarters => new List<AttributesType>(Enum.GetValues(typeof(AttributesType)).Cast<AttributesType>()).Except(new List<AttributesType>() { AttributesType.CerealConsumption, AttributesType.MeatConsumption }),
                GuildBuildingsType.ExplorationBoard => new List<AttributesType>()
                {
                    AttributesType.Discovery,
                    AttributesType.Detection,
                    AttributesType.LightWoodProduction,
                    AttributesType.HeavyWoodProduction,
                    AttributesType.IronOreProduction,
                    AttributesType.TitaniumProduction,
                    AttributesType.MeatProduction,
                    AttributesType.CerealProduction,
                },
                GuildBuildingsType.MechBoard => new List<AttributesType>() { AttributesType.MechProduction },
                GuildBuildingsType.FightBoard => new List<AttributesType>() { AttributesType.Attack },
                GuildBuildingsType.TransportBoard => new List<AttributesType>() { AttributesType.MoveSpeed },
                _ => throw ExceptionMessages.EnumOutOfRangeException(nameof(GuildBuildingsType)),
            };
        }
    }
}