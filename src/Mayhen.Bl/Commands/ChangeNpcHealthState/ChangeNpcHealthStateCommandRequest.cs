using Mayhem.Dal.Dto.Enums.Dictionaries;
using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.ChangeNpcHealthState
{
    public class ChangeNpcHealthStateCommandRequest : IRequest<ChangeNpcHealthStateCommandResponse>
    {
        public NpcHealthsState NpcHealthState { get; set; }
        public long NpcId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
