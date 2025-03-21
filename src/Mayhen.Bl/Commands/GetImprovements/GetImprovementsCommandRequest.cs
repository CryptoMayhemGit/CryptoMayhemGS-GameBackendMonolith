using MediatR;

namespace Mayhen.Bl.Commands.GetImprovements
{
    public class GetImprovementsCommandRequest : IRequest<GetImprovementsCommandResponse>
    {
        public long LandId { get; set; }

        public GetImprovementsCommandRequest(long landId)
        {
            LandId = landId;
        }
    }
}
