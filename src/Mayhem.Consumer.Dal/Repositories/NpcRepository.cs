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
    public class NpcRepository : INpcRepository
    {
        private readonly string mayhemConnectionString;
        private readonly ILogger<NpcRepository> logger;
        private readonly IDapperWrapper dapperWrapper;

        public NpcRepository(
            IMayhemConfigurationService mayhemConfigurationService,
            IDapperWrapper dapperWrapper,
            ILogger<NpcRepository> logger)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
            this.dapperWrapper = dapperWrapper;
            this.logger = logger;
        }

        public async Task<bool> UpdateNpcOwnerAsync(long id, string walletAddress)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    IdDto npc = await dapperWrapper.QueryFirstOrDefaultAsync<IdDto>(db, SqlQuerries.GetNpcIdWhereIdSql, new { id });

                    if (npc == null)
                    {
                        logger.LogError(LoggerMessages.CannotFindNftNpcWithId(id));
                        return false;
                    }

                    IdDto user = await dapperWrapper.QueryFirstAsync<IdDto>(db, SqlQuerries.GetGameUserWhereWalletAddressSql, new { WalletAddress = walletAddress });

                    if (user == null)
                    {
                        logger.LogInformation(LoggerMessages.CannotFindUserWithWallet(walletAddress));
                        return false;
                    }

                    await dapperWrapper.QueryAsync(db, SqlQuerries.UpdateNpcUserIdSql, new { UserId = user.Id, npc.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(UpdateNpcOwnerAsync)));
                    return false;
                }

                return true;
            }
        }

        public async Task<bool> RemoveNpcFromUserAsync(long id)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    IdDto npc = await dapperWrapper.QueryFirstOrDefaultAsync<IdDto>(db, SqlQuerries.GetNpcIdWhereIdSql, new { id });

                    if (npc == null)
                    {
                        logger.LogError(LoggerMessages.CannotFindNftNpcWithId(id));
                        return false;
                    }

                    await dapperWrapper.QueryAsync(db, SqlQuerries.UpdateNpcUserIdNullSql, new { npc.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(RemoveNpcFromUserAsync)));
                    return false;
                }

                return true;
            }
        }
    }
}
