using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetLandDetails
{
    public class GetLandDetailsCommandHandler : IRequestHandler<GetLandDetailsCommandRequest, GetLandDetailsCommandResponse>
    {
        private readonly IUserLandRepository userLandRepository;

        public GetLandDetailsCommandHandler(IUserLandRepository userLandRepository)
        {
            this.userLandRepository = userLandRepository;
        }

        public async Task<GetLandDetailsCommandResponse> Handle(GetLandDetailsCommandRequest request, CancellationToken cancellationToken)
        {
            UserLandDto land = await userLandRepository.GetUserLandAsync(request.LandId);

            if (land == null)
            {
                return null;
            }

            return new GetLandDetailsCommandResponse()
            {
                UserLand = land,
            };
        }
    }
}
