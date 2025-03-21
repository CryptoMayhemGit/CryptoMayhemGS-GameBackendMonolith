using Mayhem.Cache;
using Mayhem.Cache.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetInstance
{
    public class GetLandInstanceCommandHandler : IRequestHandler<GetLandInstanceCommandRequest, GetLandInstanceCommandResponse>
    {
        private readonly ILandRepository landRepository;
        private readonly ICacheService cacheService;

        public GetLandInstanceCommandHandler(ILandRepository landRepository, ICacheService cacheService)
        {
            this.landRepository = landRepository;
            this.cacheService = cacheService;
        }

        public async Task<GetLandInstanceCommandResponse> Handle(GetLandInstanceCommandRequest request, CancellationToken cancellationToken)
        {
            string cacheKey = CacheKeys.GetPlanetInstanceKey(request.InstanceId);

            IEnumerable<SimpleLandDto> lands = await cacheService.GetObjectAsync<IEnumerable<SimpleLandDto>>(cacheKey);
            if (lands == null)
            {
                lands = await landRepository.GetSimpleLandsByInstanceIdAsync(request.InstanceId);
                await cacheService.SetObjectAsync(cacheKey, lands);
            }

            return new GetLandInstanceCommandResponse()
            {
                Lands = lands,
            };
        }
    }
}
