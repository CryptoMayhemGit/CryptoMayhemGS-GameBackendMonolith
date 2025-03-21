using MediatR;
using System.Text.Json.Serialization;

namespace Mayhen.Bl.Commands.GetAvailableItems
{
    public class GetAvailableItemsCommandRequest : IRequest<GetAvailableItemsCommandResponse>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public GetAvailableItemsCommandRequest(int userId)
        {
            UserId = userId;
        }
    }
}
