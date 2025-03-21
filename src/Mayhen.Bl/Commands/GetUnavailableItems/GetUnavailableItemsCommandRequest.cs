using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.GetUnavailableItems
{
    public class GetUnavailableItemsCommandRequest : IRequest<GetUnavailableItemsCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public GetUnavailableItemsCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
