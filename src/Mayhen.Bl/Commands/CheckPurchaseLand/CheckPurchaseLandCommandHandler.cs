using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CheckPurchaseLand
{
    public class CheckPurchaseLandCommandHandler : IRequestHandler<CheckPurchaseLandCommandRequest, CheckPurchaseLandCommandResponse>
    {
        private readonly IUserLandRepository userLandRepository;

        public CheckPurchaseLandCommandHandler(IUserLandRepository userLandRepository)
        {
            this.userLandRepository = userLandRepository;
        }

        public async Task<CheckPurchaseLandCommandResponse> Handle(CheckPurchaseLandCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await userLandRepository.CheckPurchaseLandAsync(request.LandId, request.UserId);

            return new CheckPurchaseLandCommandResponse()
            {
                Result = result,
            };
        }
    }
}
