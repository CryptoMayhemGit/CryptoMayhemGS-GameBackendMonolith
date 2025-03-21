using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;

namespace Mayhen.Bl.Commands.CheckImprovement
{
    public class CheckImprovementCommandRequest : IRequest<CheckImprovementCommandResponse>
    {
        public long LandId { get; set; }
        public int Level { get; set; }
        public BuildingsType BuildingsTypeId { get; set; }
    }
}
