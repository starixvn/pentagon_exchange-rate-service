using MongoDB.Driver;

namespace ExchangeRate.Subscribers.VG
{
    public interface IVGMongoClient
    {
        IMongoDatabase GetDatabase();
    }

    public class VGMongoClient : IVGMongoClient
    {
        private readonly IMongoDatabase database;

        public VGMongoClient(SubcriberDatabase dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.ConnectionString);
            database = mongoClient.GetDatabase(dbSettings.DatabaseName);
        }

        public IMongoDatabase GetDatabase() => database;
    }
}