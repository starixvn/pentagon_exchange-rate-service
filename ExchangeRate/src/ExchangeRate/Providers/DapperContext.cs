using System.Data;
using Microsoft.Data.SqlClient;

namespace ExchangeRate.Providers
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
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
    }
}