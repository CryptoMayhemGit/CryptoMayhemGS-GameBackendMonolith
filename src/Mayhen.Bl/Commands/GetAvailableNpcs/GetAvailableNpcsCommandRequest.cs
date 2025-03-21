using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.GetAvailableNpcs
{
    public class GetAvailableNpcsCommandRequest : IRequest<GetAvailableNpcsCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public GetAvailableNpcsCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
