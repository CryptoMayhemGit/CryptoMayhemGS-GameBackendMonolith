using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Messages;
using Mayhen.Bl.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Services.Implementations
{
    public class DamageService : IDamageService
    {
        private const int DecimalPlacesNumber = 6;

        public ICollection<AttributesType> AttributesToEdit => new List<AttributesType>()
        {
            AttributesType.LightWoodProduction,
            AttributesType.HeavyWoodProduction,
            AttributesType.IronOreProduction,
            AttributesType.TitaniumProduction,
            AttributesType.MeatProduction,
            AttributesType.CerealProduction,
            AttributesType.Attack,
            AttributesType.MoveSpeed,
            AttributesType.Discovery,
            AttributesType.Repair,
            AttributesType.Construction,
            AttributesType.Detection,
            AttributesType.MechProduction,
        };

        public void SetAttributesBasedOnHealth(List<AttributeDto> attributeDtos, NpcHealthsState currentHealthsType, NpcHealthsState newHealthsType)
        {
            switch (currentHealthsType)
            {
                case NpcHealthsState.Healthy:
                    switch (newHealthsType)
                    {
                        case NpcHealthsState.Healthy:
                            break;
                        case NpcHealthsState.Wounded:
                            for (int i = 0; i < attributeDtos.Count; i++)
                            {
                                if (AttributesToEdit.Contains(attributeDtos[i].AttributeTypeId))
                                {
                                    attributeDtos[i].Value = Math.Round(attributeDtos[i].Value * 0.5d, DecimalPlacesNumber);
                                }
                            }
                            break;
                        case NpcHealthsState.Dying:
                            foreach (AttributeDto item in attributeDtos.Where(x => AttributesToEdit.Contains(x.AttributeTypeId)))
                            {
                                item.Value = Math.Round(item.Value * 0.1, DecimalPlacesNumber);
                            }
                            break;
                    }
                    break;
                case NpcHealthsState.Wounded:
                    switch (newHealthsType)
                    {
                        case NpcHealthsState.Healthy:
                            foreach (AttributeDto item in attributeDtos.Where(x => AttributesToEdit.Contains(x.AttributeTypeId)))
                            {
                                item.Value = Math.Round(item.Value * 2, DecimalPlacesNumber);
                            }
                            break;
                        case NpcHealthsState.Wounded:
                            break;
                        case NpcHealthsState.Dying:
                            foreach (AttributeDto item in attributeDtos.Where(x => AttributesToEdit.Contains(x.AttributeTypeId)))
                            {
                                item.Value = Math.Round(item.Value * 0.2, DecimalPlacesNumber);
                            }
                            break;
                    }
                    break;
                case NpcHealthsState.Dying:
                    switch (newHealthsType)
                    {
                        case NpcHealthsState.Healthy:
                            foreach (AttributeDto item in attributeDtos.Where(x => AttributesToEdit.Contains(x.AttributeTypeId)))
                            {
                                item.Value = Math.Round(item.Value * 10, DecimalPlacesNumber);
                            }
                            break;
                        case NpcHealthsState.Wounded:
                            foreach (AttributeDto item in attributeDtos.Where(x => AttributesToEdit.Contains(x.AttributeTypeId)))
                            {
                                item.Value = Math.Round(item.Value * 5, DecimalPlacesNumber);
                            }
                            break;
                        case NpcHealthsState.Dying:
                            break;
                    }
                    break;
                default:
                    throw ExceptionMessages.EnumOutOfRangeException(nameof(NpcHealthsState));
            }
        }
    }
}
