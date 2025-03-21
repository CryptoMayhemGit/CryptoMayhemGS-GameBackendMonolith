using AutoMapper;
using Mayhem.Dal.Dto.Classes.Improvements;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class ImprovementRepository : IImprovementRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public ImprovementRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ImprovementDto>> GetImprovementsByLandIdAsync(long landId) => await mayhemDataContext
            .Improvements
            .AsNoTracking()
            .Where(x => x.LandId == landId)
            .Select(x => mapper.Map<ImprovementDto>(x))
            .ToListAsync();

        public async Task<IEnumerable<GuildImprovementDto>> GetGuildImprovementsByGuildIdAsync(long guildId) => await mayhemDataContext
            .GuildImprovements
            .AsNoTracking()
            .Where(x => x.GuildId == guildId)
            .Select(x => mapper.Map<GuildImprovementDto>(x))
            .ToListAsync();

        public async Task<ImprovementDto> AddImprovementAsync(ImprovementDto improvementDto, int userId)
        {
            Dictionary<ResourcesType, int> costs = ImprovementCostsDictionary.GetImprovementCosts(improvementDto.ImprovementTypeId);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                List<UserResource> resources = await mayhemDataContext.UserResources.Where(x => x.UserId == userId).ToListAsync();

                foreach (KeyValuePair<ResourcesType, int> cost in costs)
                {
                    UserResource resource = resources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                    resource.Value -= cost.Value;
                }
                EntityEntry<Improvement> addedImprovement = await mayhemDataContext.Improvements.AddAsync(mapper.Map<Improvement>(improvementDto));
                await mayhemDataContext.SaveChangesAsync();

                await ts.CommitAsync();

                return mapper.Map<ImprovementDto>(addedImprovement.Entity);
            }
            catch (ValidationException)
            {
                await ts.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(AddImprovementAsync));
            }
        }

        public async Task<GuildImprovementDto> AddGuildImprovementAsync(GuildImprovementDto guildImprovementDto)
        {
            Guild guild = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == guildImprovementDto.GuildId);

            Dictionary<ResourcesType, int> costs = GuildImprovementCostsDictionary.GetGuildImprovementCosts(guildImprovementDto.GuildImprovementTypeId);

            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                List<GuildResource> resources = await mayhemDataContext.GuildResources.Where(x => x.GuildId == guild.Id).ToListAsync();

                foreach (KeyValuePair<ResourcesType, int> cost in costs)
                {
                    GuildResource resource = resources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                    resource.Value -= cost.Value;
                }
                EntityEntry<GuildImprovement> addedImprovement = await mayhemDataContext.GuildImprovements.AddAsync(mapper.Map<GuildImprovement>(guildImprovementDto));
                await mayhemDataContext.SaveChangesAsync();

                await ts.CommitAsync();

                return mapper.Map<GuildImprovementDto>(addedImprovement.Entity);
            }
            catch (ValidationException)
            {
                await ts.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(AddGuildImprovementAsync));
            }
        }
    }
}
