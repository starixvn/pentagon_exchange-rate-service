using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ExchangeRate.Subscribers
{
    public interface IExchangeDataAccess
    {
        Task Insert(ExchangeRateLog exchangeRateLog);

        Task<int?> GetLastExchangeId();
    }

    public class ExchangeDataAccess : IExchangeDataAccess
    {
        protected IMongoCollection<ExchangeRateLog> exchangeCollection;

        protected ExchangeDataAccess()
        {
        }

        public Task Insert(ExchangeRateLog exchangeRateLog)
        => exchangeCollection.InsertOneAsync(exchangeRateLog);

        public async Task<int?> GetLastExchangeId()
        {
            var options = new FindOptions<ExchangeRateLog, ExchangeRateLog>
            {
                Limit = 1,
                Sort = Builders<ExchangeRateLog>.Sort.Descending(x => x.ExId)
            };

            return (await exchangeCollection.FindAsync(x => true, options)).FirstOrDefault()?.ExId;
        }
    }
}