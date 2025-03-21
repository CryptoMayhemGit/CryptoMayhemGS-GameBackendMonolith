using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.CheckPurchaseLand
{
    public class CheckPurchaseLandCommandRequest : IRequest<CheckPurchaseLandCommandResponse>
    {
        public long LandId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        public CheckPurchaseLandCommandRequest(long landId, int userId)
        {
            LandId = landId;
            UserId = userId;
        }
    }
}
