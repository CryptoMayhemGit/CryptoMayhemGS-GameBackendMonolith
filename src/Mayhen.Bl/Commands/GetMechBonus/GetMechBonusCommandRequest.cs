using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.GetMechBonus
{
    public class GetMechBonusCommandRequest : IRequest<GetMechBonusCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public GetMechBonusCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
