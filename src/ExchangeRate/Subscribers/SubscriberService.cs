using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRate.Subscribers
{
    public interface ISubscriberService
    {
        Task InsertExchangeRates(string source, IEnumerable<Providers.ExchangeRateRecord> exchangeRates);
    }

    public class SubscriberService : ISubscriberService
    {
        protected readonly string baseCode;
        protected readonly IExchangeDataAccess exchangeDataAccess;
        protected readonly IGameConfigDataAccess gameConfigDataAccess;
        protected readonly ISentryClient sentryClient;

        protected SubscriberService(string baseCode, IExchangeDataAccess exchangeDataAccess, IGameConfigDataAccess gameConfigDataAccess, ISentryClient sentryClient)
        {
            this.exchangeDataAccess = exchangeDataAccess;
            this.gameConfigDataAccess = gameConfigDataAccess;
            this.baseCode = baseCode;
            this.sentryClient = sentryClient;
        }

        public async Task InsertExchangeRates(string source, IEnumerable<Providers.ExchangeRateRecord> exchangeRates)
        {
            if (exchangeRates.Any())
            {
                var exchangeLog = new ExchangeRateLog
                {
                    Time = DateTimeOffset.UtcNow.DateTime,
                    ExDate = exchangeRates.First().ExchangeDate,
                    Source = source,
                    ExId = await GetNextId(),
                    Code = ConvertCurrencyCodes(exchangeRates.Where(ex => ex.BaseCode.Equals(baseCode, StringComparison.OrdinalIgnoreCase)))
                };

                if (exchangeLog.Code.Count > 0)
                {
                    await exchangeDataAccess.Insert(exchangeLog);
                    await gameConfigDataAccess.UpdateReloadExchange();
                }
                else
                {
                    sentryClient.CaptureMessage($"{DateTime.Now} - Exchange got empty codes", Sentry.Protocol.SentryLevel.Fatal);
                }
            }
        }

        private async Task<int> GetNextId()
        {
            var latestExchange = await exchangeDataAccess.GetLastExchange();

            return latestExchange == null ? 1 : latestExchange.ExId + 1;
        }

        private static Dictionary<string, decimal> ConvertCurrencyCodes(IEnumerable<Providers.ExchangeRateRecord> exchangeRates)
        {
            var codes = new Dictionary<string, decimal>();

            foreach (var rate in exchangeRates)
            {
                if (!string.IsNullOrWhiteSpace(rate.Code))
                {
                    codes.Add(rate.Code, rate.BaseRate);
                }
            }

            return codes;
        }
    }
}