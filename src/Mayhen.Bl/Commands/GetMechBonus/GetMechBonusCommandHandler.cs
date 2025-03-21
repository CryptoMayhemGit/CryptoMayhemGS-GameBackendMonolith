using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetMechBonus
{
    public class GetMechBonusCommandHandler : IRequestHandler<GetMechBonusCommandRequest, GetMechBonusCommandResponse>
    {
        private readonly IBonusRepository bonusRepository;

        public GetMechBonusCommandHandler(IBonusRepository bonusRepository)
        {
            this.bonusRepository = bonusRepository;
        }

        public async Task<GetMechBonusCommandResponse> Handle(GetMechBonusCommandRequest request, CancellationToken cancellationToken)
        {
            double bonus = await bonusRepository.GetMechBonusByUserIdAsync(request.UserId);

            return new GetMechBonusCommandResponse()
            {
                Bonus = bonus,
            };
        }
    }
}
