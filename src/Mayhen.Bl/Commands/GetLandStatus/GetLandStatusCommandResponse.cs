using Mayhem.Dal.Dto.Dtos;
using System.Collections.Generic;

namespace Mayhen.Bl.Commands.GetLandStatus
{
    public class GetLandStatusCommandResponse
    {
        public GetLandStatusCommandResponse()
        {
            Operations = new List<LandOperations<object>>();
        }

        public List<LandOperations<object>> Operations { get; set; }
    }
}
