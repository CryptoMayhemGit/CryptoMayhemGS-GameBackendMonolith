using Mayhem.Configuration.Interfaces;
using Mayhem.Consumer.Dal.Dto.Dtos;
using Mayhem.Consumer.Dal.Interfaces.Repositories;
using Mayhem.Consumer.Dal.Interfaces.Wrapers;
using Mayhem.Messages;
using Mayhem.SqlDapper;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace Mayhem.Consumer.Dal.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly string mayhemConnectionString;
        private readonly ILogger<ItemRepository> logger;
        private readonly IDapperWrapper dapperWrapper;

        public ItemRepository(
            IMayhemConfigurationService mayhemConfigurationService,
            IDapperWrapper dapperWrapper,
            ILogger<ItemRepository> logger)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
            this.dapperWrapper = dapperWrapper;
            this.logger = logger;
        }

        public async Task<bool> UpdateItemOwnerAsync(long id, string walletAddress)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    IdDto item = await dapperWrapper.QueryFirstOrDefaultAsync<IdDto>(db, SqlQuerries.GetItemWhereIdSql, new { id });

                    if (item == null)
                    {
                        logger.LogError(LoggerMessages.CannotFindNftItemWithId(id));
                        return false;
                    }


                    IdDto user = await dapperWrapper.QueryFirstAsync<IdDto>(db, SqlQuerries.GetGameUserWhereWalletAddressSql, new { WalletAddress = walletAddress });

                    if (user == null)
                    {
                        logger.LogInformation(LoggerMessages.CannotFindUserWithWallet(walletAddress));
                        return false;
                    }

                    await dapperWrapper.QueryAsync(db, SqlQuerries.UpdateItemWhereUserIdSql, new { UserId = user.Id, item.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(UpdateItemOwnerAsync)));
                    return false;
                }

                return true;
            }
        }

        public async Task<bool> RemoveItemFromUserAsync(long id)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    IdDto item = await dapperWrapper.QueryFirstOrDefaultAsync<IdDto>(db, SqlQuerries.GetItemWhereIdSql, new { id });

                    if (item == null)
                    {
                        logger.LogError(LoggerMessages.CannotFindNftItemWithId(id));
                        return false;
                    }

                    await dapperWrapper.QueryAsync(db, SqlQuerries.UpdateItemWhereUserIdNullSql, new { item.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(RemoveItemFromUserAsync)));
                    return false;
                }

                return true;
            }
        }
    }
}
