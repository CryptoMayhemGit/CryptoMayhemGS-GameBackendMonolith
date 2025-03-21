using AutoMapper;
using Mayhem.Dal.Dto.Commands.GetUser;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetUser
{
    public class GetUserCommandHandler : IRequestHandler<GetUserCommandRequest, GetUserCommandResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<GetUserCommandResponse> Handle(GetUserCommandRequest request, CancellationToken cancellationToken)
        {
            GetUserCommandResponseDto getUserCommandResponse = await userRepository.GetUserAsync(mapper.Map<GetUserCommandRequestDto>(request));

            return mapper.Map<GetUserCommandResponse>(getUserCommandResponse);
        }
    }
}
