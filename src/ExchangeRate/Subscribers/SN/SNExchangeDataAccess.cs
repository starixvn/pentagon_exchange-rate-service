namespace ExchangeRate.Subscribers.SN
{
    public interface ISNExchangeDataAccess : IExchangeDataAccess
    {
    }

    public class SNExchangeDataAccess : ExchangeDataAccess, ISNExchangeDataAccess
    {
        private const string ExchangeCollectionName = "exchange_rates";

        public SNExchangeDataAccess(ISNMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase();

            exchangeCollection = database.GetCollection<ExchangeRateLog>(ExchangeCollectionName);
        }
    }
}