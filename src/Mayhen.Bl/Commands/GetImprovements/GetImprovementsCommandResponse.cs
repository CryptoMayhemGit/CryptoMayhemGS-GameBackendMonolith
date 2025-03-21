using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetImprovements
{
    public class GetImprovementsCommandResponse
    {
        public IEnumerable<ImprovementDto> Improvements { get; set; }
    }
}
