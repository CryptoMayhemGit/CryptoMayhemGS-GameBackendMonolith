using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mayhem.Consumer.Dal.Interfaces.Wrapers
{
    /// <summary>
    /// Dapper Wrapper
    /// </summary>
    public interface IDapperWrapper
    {
        /// <summary>
        /// Queries the first or default asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
        /// <summary>
        /// Queries the first asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        Task<T> QueryFirstAsync<T>(IDbConnection connection, string sql, object param = null);
        /// <summary>
        /// Queries the asynchronous.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryAsync(IDbConnection connection, string sql, object param = null);
    }
}
