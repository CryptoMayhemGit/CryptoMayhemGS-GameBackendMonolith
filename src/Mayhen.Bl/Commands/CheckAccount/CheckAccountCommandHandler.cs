using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CheckAccount
{
    public class CheckAccountCommandHandler : IRequestHandler<CheckAccountCommandRequest>
    {
        public Task<Unit> Handle(CheckAccountCommandRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}
