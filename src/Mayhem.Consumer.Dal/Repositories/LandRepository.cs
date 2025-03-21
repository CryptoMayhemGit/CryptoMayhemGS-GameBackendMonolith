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
    public class LandRepository : ILandRepository
    {
        private readonly string mayhemConnectionString;
        private readonly ILogger<LandRepository> logger;
        private readonly IDapperWrapper dapperWrapper;

        public LandRepository(
            IMayhemConfigurationService mayhemConfigurationService,
            IDapperWrapper dapperWrapper,
            ILogger<LandRepository> logger)
        {
            mayhemConnectionString = mayhemConfigurationService.MayhemConfiguration.ConnectionStringsConfigruation.MSSQLConnectionString;
            this.dapperWrapper = dapperWrapper;
            this.logger = logger;
        }

        public async Task<bool> UpdateLandOwnerAsync(long id, string walletAddress)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    IdDto land = await dapperWrapper.QueryFirstOrDefaultAsync<IdDto>(db, SqlQuerries.GetLandWhereIdSql, new { id });

                    if (land == null)
                    {
                        logger.LogError(LoggerMessages.CannotFindNftLandWithId(id));
                        return false;
                    }


                    IdDto user = await dapperWrapper.QueryFirstAsync<IdDto>(db, SqlQuerries.GetGameUserWhereWalletAddressSql, new { WalletAddress = walletAddress });

                    if (user == null)
                    {
                        logger.LogInformation(LoggerMessages.CannotFindUserWithWallet(walletAddress));
                        return false;
                    }

                    await dapperWrapper.QueryAsync(db, SqlQuerries.UpdateLandUserIdSql, new { UserId = user.Id, land.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(UpdateLandOwnerAsync)));
                    return false;
                }

                return true;
            }
        }

        public async Task<bool> RemoveLandFromUserAsync(long id)
        {
            using (IDbConnection db = new SqlConnection(mayhemConnectionString))
            {
                try
                {
                    IdDto land = await dapperWrapper.QueryFirstOrDefaultAsync<IdDto>(db, SqlQuerries.GetLandWhereIdSql, new { id });

                    if (land == null)
                    {
                        logger.LogError(LoggerMessages.CannotFindNftLandWithId(id));
                        return false;
                    }

                    await dapperWrapper.QueryAsync(db, SqlQuerries.UpdateLandUserIdNullSql, new { land.Id });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, LoggerMessages.ErrorOccurredDuring(nameof(RemoveLandFromUserAsync)));
                    return false;
                }

                return true;
            }
        }
    }
}
