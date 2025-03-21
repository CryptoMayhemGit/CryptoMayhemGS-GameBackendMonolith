using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetAvailableNpcs
{
    public class GetAvailableNpcsCommandResponse
    {
        public IEnumerable<NpcDto> Npcs { get; set; }
    }
}
