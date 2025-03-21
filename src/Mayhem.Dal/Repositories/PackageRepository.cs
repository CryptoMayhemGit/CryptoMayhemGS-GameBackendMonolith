using AutoMapper;
using Mayhem.Dal.Dto.Classes.Generator;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Messages;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;
        private readonly ILogger<PackageRepository> logger;

        public PackageRepository(IMayhemDataContext mayhemDataContext,
                                 IMapper mapper,
                                 ILogger<PackageRepository> logger)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task AddItemWithNpcAsync(List<Package> packagesList)
        {
            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {

                mayhemDataContext.ChangeTracker.AutoDetectChangesEnabled = false;
                for (int i = 0; i < packagesList.Count; i++)
                {
                    for (int j = 0; j < packagesList[i].Npcs.Count; j++)
                    {
                        await mayhemDataContext.Npcs.AddAsync(mapper.Map<Tables.Nfts.Npc>(packagesList[i].Npcs[j]));
                        await mayhemDataContext.SaveChangesAsync();
                    }
                    for (int j = 0; j < packagesList[i].MatchingItems.Count; j++)
                    {
                        await mayhemDataContext.Items.AddAsync(mapper.Map<Tables.Nfts.Item>(packagesList[i].MatchingItems[j]));
                        await mayhemDataContext.SaveChangesAsync();
                    }
                    for (int j = 0; j < packagesList[i].MismatchingItems.Count; j++)
                    {
                        await mayhemDataContext.Items.AddAsync(mapper.Map<Tables.Nfts.Item>(packagesList[i].MismatchingItems[j]));
                        await mayhemDataContext.SaveChangesAsync();
                    }
                }
                await ts.CommitAsync();
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(AddItemWithNpcAsync)));
            }
            finally
            {
                mayhemDataContext.ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
    }
}
