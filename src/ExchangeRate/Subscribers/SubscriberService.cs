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
        protected readonly IExchangeDataAccess exchangeDataAccess;
        protected readonly IGameConfigDataAccess gameConfigDataAccess;

        protected SubscriberService(IExchangeDataAccess exchangeDataAccess, IGameConfigDataAccess gameConfigDataAccess)
        {
            this.exchangeDataAccess = exchangeDataAccess;
            this.gameConfigDataAccess = gameConfigDataAccess;
        }

        public async Task InsertExchangeRates(string source, IEnumerable<Providers.ExchangeRateRecord> exchangeRates)
        {
            if (exchangeRates.Any())
            {
                var exchangeLog = new ExchangeRateLog
                {
                    Time = DateTimeOffset.UtcNow.DateTime,
                    ExDate = exchangeRates.FirstOrDefault().ExchangeDate,
                    Source = source,
                    ExId = await GetNextId(),
                    Code = ConvertCurrencyCodes(exchangeRates)
                };

                await exchangeDataAccess.Insert(exchangeLog);
                await gameConfigDataAccess.UpdateReloadExchange();
            }
        }

        private async Task<int> GetNextId() 
        {
            var lastId = await exchangeDataAccess.GetLastExchangeId();

            return lastId == null ? 1 : (int)lastId + 1;
        }

        private static Dictionary<string, decimal> ConvertCurrencyCodes(IEnumerable<Providers.ExchangeRateRecord> exchangeRates)
        {
            var codes = new Dictionary<string, decimal>();

            foreach (var rate in exchangeRates)
            {
                if (!string.IsNullOrWhiteSpace(rate.Code))
                {
                    codes.Add(rate.Code, rate.USD);
                }
            }

            return codes;
        }
    }
}