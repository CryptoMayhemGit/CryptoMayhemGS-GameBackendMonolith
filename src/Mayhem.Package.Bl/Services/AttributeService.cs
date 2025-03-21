
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;
using Mayhem.Package.Bl.Interfaces;

namespace Mayhem.Package.Bl.Services
{
    public class AttributeService : IAttributeService
    {
        public ICollection<AttributeDto> GetBasicNpcAttributesWithValue(NpcsType npcType)
        {
            return npcType switch
            {
                NpcsType.Miner => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Lumberjack => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 1.5,
                            BaseValue = 1.5,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.8,
                            BaseValue = 0.8
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Hunter => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 2.2,
                            BaseValue = 2.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Farmer => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 2.2,
                            BaseValue = 2.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.6,
                            BaseValue = 0.6,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Soldier => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 2,
                            BaseValue = 2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Scout => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1.5,
                            BaseValue = 1.5,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Mechanic => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Engineer => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Biologist => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.8,
                            BaseValue = 1.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.4,
                            BaseValue = 0.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.8,
                            BaseValue = 1.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Chemist => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.9,
                            BaseValue = 0.9,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.3,
                            BaseValue = 1.3,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Pilot => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.1,
                            BaseValue = 1.1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 2,
                            BaseValue = 2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Doctor => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.2,
                            BaseValue = 1.2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 2,
                            BaseValue = 2,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Geologist => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                NpcsType.Fitter => new List<AttributeDto>()
                    {
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.LightWoodProduction,
                            Value = 0.75,
                            BaseValue = 0.75,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.HeavyWoodProduction,
                            Value = 0.4,
                            BaseValue = 0.4
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.IronOreProduction,
                            Value = 0.7,
                            BaseValue = 0.7,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.TitaniumProduction,
                            Value = 0.35,
                            BaseValue = 0.35,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatProduction,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealProduction,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MeatConsumption,
                            Value = 0.8,
                            BaseValue = 0.8,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.CerealConsumption,
                            Value = 1.4,
                            BaseValue = 1.4,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Attack,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Healing,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MoveSpeed,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Discovery,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Repair,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Construction,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.Detection,
                            Value = 1,
                            BaseValue = 1,
                        },
                        new AttributeDto()
                        {
                            AttributeTypeId = AttributesType.MechProduction,
                            Value = 1,
                            BaseValue = 1,
                        },
                    },
                _ => new List<AttributeDto>(),
            };
        }
    }
}
