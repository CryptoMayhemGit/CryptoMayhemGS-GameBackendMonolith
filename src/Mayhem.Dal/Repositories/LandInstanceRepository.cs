using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class LandInstanceRepository : ILandInstanceRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public LandInstanceRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<LandInstanceDto> GetLastInsanceAsync() => await mayhemDataContext
            .LandInstances
            .Include(x => x.Lands)
            .Select(x => mapper.Map<LandInstanceDto>(x))
            .LastOrDefaultAsync();

        public async Task<LandInstanceDto> AddInstanceAsync()
        {
            EntityEntry<LandInstance> newInstance = await mayhemDataContext.LandInstances.AddAsync(new LandInstance());
            await mayhemDataContext.SaveChangesAsync();

            return mapper.Map<LandInstanceDto>(newInstance.Entity);
        }

        public async Task<IEnumerable<LandInstanceDto>> AddInstancesAsync(int count)
        {
            List<LandInstance> list = new();
            for (int i = 0; i < count; i++)
            {
                list.Add(new LandInstance());
            }

            await mayhemDataContext.LandInstances.AddRangeAsync(list);
            await mayhemDataContext.SaveChangesAsync();

            return list.Select(x => mapper.Map<LandInstanceDto>(x));
        }
    }
}
