using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public JobRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<JobDto>> GetJobsByLandIdAsync(long landId) => await mayhemDataContext
                .Jobs
                .AsNoTracking()
                .Where(x => x.LandId == landId)
                .Select(x => mapper.Map<JobDto>(x))
                .ToListAsync();
    }
}
