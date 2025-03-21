using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using System.Collections.Generic;

namespace Mayhem.Package.Bl.Interfaces
{
    /// <summary>
    /// Attribute Service
    /// </summary>
    public interface IAttributeService
    {
        /// <summary>
        /// Gets the basic NPC attributes with value.
        /// </summary>
        /// <param name="npcType">Type of the NPC.</param>
        /// <returns></returns>
        ICollection<AttributeDto> GetBasicNpcAttributesWithValue(NpcsType npcType);
    }
}
