using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ExchangeRate.Subscribers
{
    public interface ISubcriberService
    {
        Task InsertExchangeRates(IEnumerable<Providers.ExchangeRate> exchangeRates);
    }

    public class SubcriberService : ISubcriberService
    {
        private const string ExchangeCollectionName = "exchange_rates";

        private readonly IMongoCollection<ExchangeRateLog> exchangeCollection;

        public SubcriberService(IMongoClient mongoClient, SubcriberSettings subcriberSettings)
        {
            var database = mongoClient.GetDatabase(subcriberSettings.SC.DatabaseName);

            exchangeCollection = database.GetCollection<ExchangeRateLog>(ExchangeCollectionName);
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
                Time = DateTimeOffset.UtcNow.DateTime,
                ExDate = exchangeRates.FirstOrDefault().ExchangeDate,
                Source = "OW",
                ExId = await GetLatestExchangeId(),
                Code = codes
            };

            await exchangeCollection.InsertOneAsync(exchangeLog);
        }

        public async Task<int> GetLatestExchangeId()
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