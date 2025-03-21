using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.GetLandStatus
{
    public class GetLandStatusCommandHandler : IRequestHandler<GetLandStatusCommandRequest, GetLandStatusCommandResponse>
    {
        private readonly IJobRepository jobRepository;
        private readonly ITravelRepository travelRepository;
        private readonly IBuildingRepository buildingRepository;

        public GetLandStatusCommandHandler(
            IJobRepository jobRepository,
            ITravelRepository travelRepository,
            IBuildingRepository buildingRepository)
        {
            this.jobRepository = jobRepository;
            this.travelRepository = travelRepository;
            this.buildingRepository = buildingRepository;
        }

        public async Task<GetLandStatusCommandResponse> Handle(GetLandStatusCommandRequest request, CancellationToken cancellationToken)
        {
            GetLandStatusCommandResponse response = new();

            IEnumerable<JobDto> jobs = await jobRepository.GetJobsByLandIdAsync(request.LandId);
            IEnumerable<TravelDto> travelsFrom = await travelRepository.GetTravelsFromByLandIdAsync(request.LandId);
            IEnumerable<TravelDto> travelsTo = await travelRepository.GetTravelsToByLandIdAsync(request.LandId);
            IEnumerable<BuildingDto> buildings = await buildingRepository.GetBuildingsByLandIdAsync(request.LandId);

            foreach (JobDto job in jobs)
            {
                response.Operations.Add(new LandOperations<object>()
                {
                    OperationType = Mayhem.Dal.Dto.Enums.Dictionaries.LandOperationsType.Job,
                    Operation = job,
                });
            }

            foreach (TravelDto travelFrom in travelsFrom)
            {
                response.Operations.Add(new LandOperations<object>()
                {
                    OperationType = Mayhem.Dal.Dto.Enums.Dictionaries.LandOperationsType.TravelFrom,
                    Operation = travelFrom,
                });
            }

            foreach (TravelDto travelto in travelsTo)
            {
                response.Operations.Add(new LandOperations<object>()
                {
                    OperationType = Mayhem.Dal.Dto.Enums.Dictionaries.LandOperationsType.TravelTo,
                    Operation = travelto,
                });
            }

            foreach (BuildingDto building in buildings)
            {
                response.Operations.Add(new LandOperations<object>()
                {
                    OperationType = Mayhem.Dal.Dto.Enums.Dictionaries.LandOperationsType.Building,
                    Operation = building,
                });
            }

            return response;
        }
    }
}
