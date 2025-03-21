using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.CheckEmail
{
    public class CheckEmailCommandHandler : IRequestHandler<CheckEmailCommandRequest, CheckEmailCommandResponse>
    {
        private readonly IUserRepository userRepository;

        public CheckEmailCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<CheckEmailCommandResponse> Handle(CheckEmailCommandRequest request, CancellationToken cancellationToken)
        {
            bool result = await userRepository.CheckEmailAsync(request.Email);

            return new CheckEmailCommandResponse()
            {
                Result = result,
            };
        }
    }
}
