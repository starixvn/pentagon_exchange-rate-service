using System.Data;
using Microsoft.Data.SqlClient;

namespace ExchangeRate.Providers
{
    public class DapperContext
    {
        private readonly string DbConn;

        public DapperContext(ExchangeProviderSettings settings)
        {
            DbConn = settings.ConnectionString;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(DbConn);
    }
}