using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhem.Package.Bl.Interfaces
{
    /// <summary>
    /// Npc Generator Service
    /// </summary>
    public interface INpcGeneratorService
    {
        /// <summary>
        /// Generats the NPCS.
        /// </summary>
        /// <returns></returns>
        IEnumerable<NpcDto> GeneratNpcs();
    }
}
