using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AddImprovement
{
    public class AddImprovementCommandRequest : IRequest<AddImprovementCommandResponse>
    {
        public long LandId { get; set; }
        public ImprovementsType ImprovementTypeId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
