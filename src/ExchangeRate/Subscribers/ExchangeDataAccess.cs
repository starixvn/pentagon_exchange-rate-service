using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ExchangeRate.Subscribers
{
    public interface IExchangeDataAccess
    {
        Task Insert(ExchangeRateLog exchangeRateLog);

        Task<ExchangeRateLog> GetLastExchange();
    }

    public class ExchangeDataAccess : IExchangeDataAccess
    {
        protected IMongoCollection<ExchangeRateLog> exchangeCollection;

        protected ExchangeDataAccess()
        {
        }

        public Task Insert(ExchangeRateLog exchangeRateLog)
        => exchangeCollection.InsertOneAsync(exchangeRateLog);

        public async Task<ExchangeRateLog> GetLastExchange()
        {
            var projection = Builders<ExchangeRateLog>.Projection
                .Include(b => b.ExId);

            var options = new FindOptions<ExchangeRateLog, ExchangeRateLog>
            {
                Limit = 1,
                Sort = Builders<ExchangeRateLog>.Sort.Descending(x => x.ExId),
                Projection = projection
            };

            return (await exchangeCollection.FindAsync(x => true, options)).FirstOrDefault();
        }
    }
}