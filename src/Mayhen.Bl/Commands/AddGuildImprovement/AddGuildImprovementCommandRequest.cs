using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.AddGuildImprovement
{
    public class AddGuildImprovementCommandRequest : IRequest<AddGuildImprovementCommandResponse>
    {
        public int GuildId { get; set; }
        public GuildImprovementsType GuildImprovementTypeId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
