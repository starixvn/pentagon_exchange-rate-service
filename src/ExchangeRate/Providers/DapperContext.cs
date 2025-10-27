using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;
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
        private const int MaxRetryAttempts = 3;
        private readonly string DbConn;
        private readonly ISentryClient sentryClient;
        private readonly ResiliencePipeline _resiliencePipeline;

        public DapperContext(ExchangeProviderSettings settings, ISentryClient sentryClient)
        {
            DbConn = settings.ConnectionString;
            this.sentryClient = sentryClient;
            
            // Polly v8: Create resilience pipeline
            _resiliencePipeline = new ResiliencePipelineBuilder()
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = MaxRetryAttempts,
                    Delay = TimeSpan.FromSeconds(2),
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = new PredicateBuilder().Handle<SqlException>(),
                    OnRetry = args =>
                    {
                        sentryClient.CaptureMessage(
                            $"Failed to get exchange rates. Exception {args.Outcome.Exception?.Message}. Retry attempt {args.AttemptNumber}. Next retry in {args.RetryDelay}.",
                            SentryLevel.Error);
                        return ValueTask.CompletedTask;
                    }
                })
                .Build();
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(DbConn);

        public async Task<IEnumerable<T>> QueryAsync<T>(string procedure, object param = null, CommandType? commandType = null)
        {
            return await _resiliencePipeline.ExecuteAsync(async cancellationToken 
                => await ExcuteQueryAsync<T>(procedure, param, commandType));
        }

        private async Task<IEnumerable<T>> ExcuteQueryAsync<T>(string procedure, object param = null, CommandType? commandType = null)
        {
            using var connection = CreateConnection();

            return await connection.QueryAsync<T>(procedure, param, commandType: commandType);
        }
    }
}