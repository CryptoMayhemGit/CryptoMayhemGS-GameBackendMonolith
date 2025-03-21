using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Tables;
using System.Collections.Generic;

namespace Mayhem.Test.Common
{
    public class AttributeHelper
    {

        public static ICollection<Attribute> GetBasicAttributesWithValue(NpcsType npcType)
        {
            return npcType switch
            {
                NpcsType.Miner => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Lumberjack => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 1.5,
                            BaseValue = 1.5,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.8,
                            BaseValue = 0.8
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Hunter => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 2.2,
                            BaseValue = 2.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Farmer => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 2.2,
                            BaseValue = 2.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.6,
                            BaseValue = 0.6,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Soldier => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 2,
                            BaseValue = 2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Scout => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1.5,
                            BaseValue = 1.5,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Mechanic => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Engineer => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Biologist => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.8,
                            BaseValue = 1.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.8,
                            BaseValue = 1.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Chemist => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Pilot => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 2,
                            BaseValue = 2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Doctor => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 2,
                            BaseValue = 2,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Geologist => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Fitter => new List<Attribute>()
                    {
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new Attribute()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                _ => new List<Attribute>(),
            };
        }
    }
}
