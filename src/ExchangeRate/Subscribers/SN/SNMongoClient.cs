using MongoDB.Driver;

namespace ExchangeRate.Subscribers.SN
{
    public interface ISNMongoClient
    {
        IMongoDatabase GetDatabase();
    }

    public class SNMongoClient : ISNMongoClient
    {
        private readonly IMongoDatabase database;

        public SNMongoClient(SubcriberDatabase dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.ConnectionString);
            database = mongoClient.GetDatabase(dbSettings.DatabaseName);
        }

        public IMongoDatabase GetDatabase() => database;
    }
}