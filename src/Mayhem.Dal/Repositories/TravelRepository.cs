using AutoMapper;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class TravelRepository : ITravelRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public TravelRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TravelDto>> GetTravelsFromByLandIdAsync(long landId) => await mayhemDataContext
            .Travels
            .AsNoTracking()
            .Where(x => x.LandFromId == landId)
            .Select(x => mapper.Map<TravelDto>(x))
            .ToListAsync();

        public async Task<IEnumerable<TravelDto>> GetTravelsToByLandIdAsync(long landId) => await mayhemDataContext
            .Travels
            .AsNoTracking()
            .Where(x => x.LandToId == landId)
            .Select(x => mapper.Map<TravelDto>(x))
            .ToListAsync();

        public async Task<IEnumerable<TravelDto>> AddTravelsAsync(IEnumerable<TravelDto> travelDtos)
        {
            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                List<EntityEntry<Travel>> addedTravels = new();

                foreach (TravelDto travelDto in travelDtos)
                {
                    addedTravels.Add(await mayhemDataContext.Travels.AddAsync(mapper.Map<Travel>(travelDto)));
                }

                Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == travelDtos.First().NpcId);
                npc.NpcStatusId = NpcsStatus.OnTravel;

                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();

                return addedTravels.Select(x => mapper.Map<TravelDto>(x.Entity));
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(AddTravelsAsync));
            }
        }

        public async Task<bool> RemoveTravelsByNpcIdAsync(long npcId)
        {
            List<Travel> travels = await mayhemDataContext.Travels.Where(x => x.NpcId == npcId).ToListAsync();
            Npc npc = await mayhemDataContext.Npcs.SingleOrDefaultAsync(x => x.Id == npcId);
            npc.NpcStatusId = NpcsStatus.None;
            mayhemDataContext.Travels.RemoveRange(travels);

            await mayhemDataContext.SaveChangesAsync();

            return true;
        }
    }
}
