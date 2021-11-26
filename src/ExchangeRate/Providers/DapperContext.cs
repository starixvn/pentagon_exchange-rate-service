using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Polly;
using Sentry;

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
        private readonly ISentryClient sentryClient;

        public DapperContext(ExchangeProviderSettings settings, ISentryClient sentryClient)
        {
            DbConn = settings.ConnectionString;
            this.sentryClient = sentryClient;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(DbConn);

        public async Task<IEnumerable<T>> QueryAsync<T>(string procedure, object param = null, CommandType? commandType = null)
        {
            var policy = Policy
              .Handle<SqlException>()
              .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, context) =>
                {
                    sentryClient.CaptureMessage($"Failed to get exchange rates. Exception {exception.Message}. Next retry is {timeSpan}.");
                }
              );

            return await policy.ExecuteAsync(async () => await ExcuteQueryAsync<T>(procedure, param, commandType));
        }

        private async Task<IEnumerable<T>> ExcuteQueryAsync<T>(string procedure, object param = null, CommandType? commandType = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>
                (procedure, param, commandType: commandType);
        }
    }
}