using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetInstance
{
    public class GetLandInstanceCommandResponse
    {
        public IEnumerable<SimpleLandDto> Lands { get; set; }
    }
}
