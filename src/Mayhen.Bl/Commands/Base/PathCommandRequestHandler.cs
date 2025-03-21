using Mayhem.Common.Services.PathFindingService.Enums;
using Mayhem.Dal.Dto.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.Base
{
    public abstract class PathCommandRequestHandler<TReq, TRes> : IRequestHandler<TReq, TRes>
        where TReq : IRequest<TRes>
    {
        public abstract Task<TRes> Handle(TReq request, CancellationToken cancellationToken);

        public int[,] CreateGridFromLands(LandPositionDto[] lands, int arraySize)
        {
            int[,] planetLandArray = new int[arraySize, arraySize];

            for (int i = 0; i < lands.Length; i++)
            {
                planetLandArray[lands[i].PositionX, lands[i].PositionY] = (int)PathFindingLandsType.PATH;
            }

            return planetLandArray;
        }
    }
}
