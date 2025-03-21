using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.RemoveTravel
{
    public class RemoveTravelCommandHandler : IRequestHandler<RemoveTravelCommandRequest, RemoveTravelCommandResponse>
    {
        private readonly ITravelRepository travelRepository;

        public RemoveTravelCommandHandler(ITravelRepository travelRepository)
        {
            this.travelRepository = travelRepository;
        }

        public async Task<RemoveTravelCommandResponse> Handle(RemoveTravelCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await travelRepository.RemoveTravelsByNpcIdAsync(request.NpcId);

            return new RemoveTravelCommandResponse()
            {
                Result = result,
            };
        }
    }
}
