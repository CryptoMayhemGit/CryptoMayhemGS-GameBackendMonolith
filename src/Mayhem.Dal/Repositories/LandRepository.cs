using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.Nfts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class LandRepository : ILandRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public LandRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;

        }
        public async Task<IEnumerable<SimpleLandDto>> GetSimpleLandsByInstanceIdAsync(int instanceId) => await mayhemDataContext
               .Lands
               .AsNoTracking()
               .Where(x => x.LandInstanceId == instanceId)
               .Select(x => mapper.Map<SimpleLandDto>(x))
               .ToListAsync();

        public async Task<LandDto> GetLandByNftIdAsync(long landNftId) => await mayhemDataContext
            .Lands
            .AsNoTracking()
            .Where(x => x.Id == landNftId)
            .Select(x => mapper.Map<LandDto>(x))
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<LandDto>> AddLandsAsync(IEnumerable<LandDto> lands)
        {
            try
            {
                mayhemDataContext.ChangeTracker.AutoDetectChangesEnabled = false;

                int counter = 0;
                List<Land> addedLands = new();

                foreach (LandDto land in lands)
                {
                    counter++;
                    EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(mapper.Map<Land>(land));
                    addedLands.Add(newLand.Entity);

                    if (counter == 10)
                    {
                        await mayhemDataContext.SaveChangesAsync();
                        counter = 0;
                    }
                }

                await mayhemDataContext.SaveChangesAsync();
                return addedLands.Select(x => mapper.Map<LandDto>(x)).ToList();
            }
            finally
            {
                mayhemDataContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
