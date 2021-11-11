using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ExchangeRate.Subscribers
{
    public interface IExchangeDataAccess
    {
        Task Insert(ExchangeRateLog exchangeRateLog);

        Task<int> GetNextId();
    }

    public abstract class ExchangeDataAccess : IExchangeDataAccess
    {
        protected IMongoCollection<ExchangeRateLog> exchangeCollection;

        protected ExchangeDataAccess()
        {
        }

        public async Task InsertExchangeRates(IEnumerable<Providers.ExchangeRate> exchangeRates)
        {
            var codes = new Dictionary<string, decimal>();

            foreach (var rate in exchangeRates)
            {
                if (!string.IsNullOrWhiteSpace(rate.Code))
                {
                    codes.Add(rate.Code, rate.USD);
                }
            }

            var exchangeLog = new ExchangeRateLog
            {
                ExDate = exchangeRates.FirstOrDefault().ExchangeDate,
                Source = "OW",
                ExId = await GetNextId(),
                Code = codes
            };

            await exchangeCollection.InsertOneAsync(exchangeLog);
        }

        public Task Insert(ExchangeRateLog exchangeRateLog)
        => exchangeCollection.InsertOneAsync(exchangeRateLog);

        public async Task<int> GetNextId()
        {
            var options = new FindOptions<ExchangeRateLog, ExchangeRateLog>
            {
                Limit = 1,
                Sort = Builders<ExchangeRateLog>.Sort.Descending(x => x.ExId)
            };

            var latestExchange = (await exchangeCollection.FindAsync(x => true, options)).FirstOrDefault();

            return latestExchange == null ? 1 : latestExchange.ExId + 1;
        }
    }
}