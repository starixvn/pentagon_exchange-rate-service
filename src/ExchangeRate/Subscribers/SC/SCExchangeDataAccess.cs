namespace ExchangeRate.Subscribers.SC
{
    public interface ISCExchangeDataAccess : IExchangeDataAccess
    {
    }

    public class SCExchangeDataAccess : ExchangeDataAccess, ISCExchangeDataAccess
    {
        private const string ExchangeCollectionName = "exchange_rates";

        public SCExchangeDataAccess(ISCMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase();

            exchangeCollection = database.GetCollection<ExchangeRateLog>(ExchangeCollectionName);
        }
    }
}