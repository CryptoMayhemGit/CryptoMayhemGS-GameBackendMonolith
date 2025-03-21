using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;

namespace Mayhen.Bl.Commands.CheckGuildImprovement
{
    public class CheckGuildImprovementCommandRequest : IRequest<CheckGuildImprovementCommandResponse>
    {
        public long GuildId { get; set; }
        public int Level { get; set; }
        public GuildBuildingsType GuildBuildingsTypeId { get; set; }
    }
}
