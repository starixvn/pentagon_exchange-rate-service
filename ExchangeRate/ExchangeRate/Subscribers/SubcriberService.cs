using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRate.Subscribers
{
    public interface ISubcriberService
    {
        Task InsertExchangeRates(string source, IEnumerable<Providers.ExchangeRate> exchangeRates);
    }

    public abstract class SubcriberService : ISubcriberService
    {
        protected readonly IExchangeDataAccess exchangeDataAccess;

        protected SubcriberService(IExchangeDataAccess exchangeDataAccess)
        {
            this.exchangeDataAccess = exchangeDataAccess;
        }

        public async Task InsertExchangeRates(string source, IEnumerable<Providers.ExchangeRate> exchangeRates)
        {
            if (exchangeRates.Any())
            {
                var exchangeLog = new ExchangeRateLog
                {
                    Time = DateTimeOffset.UtcNow.DateTime,
                    ExDate = exchangeRates.FirstOrDefault().ExchangeDate,
                    Source = source,
                    ExId = await exchangeDataAccess.GetNextId(),
                    Code = ConvertCurrencyCodes(exchangeRates)
                };

                await exchangeDataAccess.Insert(exchangeLog);
            }
        }

        private static Dictionary<string, decimal> ConvertCurrencyCodes(IEnumerable<Providers.ExchangeRate> exchangeRates)
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