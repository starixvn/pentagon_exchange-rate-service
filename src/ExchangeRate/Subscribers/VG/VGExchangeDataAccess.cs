namespace ExchangeRate.Subscribers.VG
{
    public interface IVGExchangeDataAccess : IExchangeDataAccess
    {
    }

    public class VGExchangeDataAccess : ExchangeDataAccess, IVGExchangeDataAccess
    {
        private const string ExchangeCollectionName = "exchange_rates";

        public VGExchangeDataAccess(IVGMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase();

            exchangeCollection = database.GetCollection<ExchangeRateLog>(ExchangeCollectionName);
        }
    }
}