using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ExchangeRate.Providers
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();

        Task<IEnumerable<T>> QueryAsync<T>(string procedure, object param = null, CommandType? commandType = null);
    }

    public class DapperContext : IDapperContext
    {
        private readonly string DbConn;

        public DapperContext(ExchangeProviderSettings settings)
        {
            DbConn = settings.ConnectionString;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(DbConn);

        public async Task<IEnumerable<T>> QueryAsync<T>(string procedure, object param = null, CommandType? commandType = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>
                (procedure, param, commandType: commandType);
        }
    }
}