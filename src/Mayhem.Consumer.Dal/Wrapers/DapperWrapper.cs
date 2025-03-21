using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Dal.Interfaces.Wrapers
{
    public class DapperWrapper : IDapperWrapper
    {
        public Task<IEnumerable<dynamic>> QueryAsync(IDbConnection connection, string sql, object param = null)
        {
            return connection.QueryAsync(sql, param);
        }

        public Task<T> QueryFirstAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return connection.QueryFirstAsync<T>(sql, param);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }
    }
}
