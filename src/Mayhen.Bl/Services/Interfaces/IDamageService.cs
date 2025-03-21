using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhen.Bl.Services.Interfaces
{
    /// <summary>
    /// Damage Service
    /// </summary>
    public interface IDamageService
    {
        /// <summary>
        /// Gets the attributes to edit.
        /// </summary>
        /// <value>
        /// The attributes to edit.
        /// </value>
        ICollection<AttributesType> AttributesToEdit { get; }
        /// <summary>
        /// Sets the attributes based on health.
        /// </summary>
        /// <param name="attributeDtos">The attribute dtos.</param>
        /// <param name="currentHealthsType">Type of the current healths.</param>
        /// <param name="newHealthsType">New type of the healths.</param>
        void SetAttributesBasedOnHealth(List<AttributeDto> attributeDtos, NpcHealthsState currentHealthsType, NpcHealthsState newHealthsType);
    }
}