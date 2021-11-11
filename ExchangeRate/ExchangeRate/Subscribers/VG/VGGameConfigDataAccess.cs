namespace ExchangeRate.Subscribers.VG
{
    public interface IVGGameConfigDataAccess : IGameConfigDataAccess
    {
    }

    public class VGGameConfigDataAccess : GameConfigDataAccess, IVGGameConfigDataAccess
    {
        private const string GameConfigCollectionName = "game_configs";

        public VGGameConfigDataAccess(IVGMongoClient mongoClient) : base()
        {
            var database = mongoClient.GetDatabase();

            gameConfigCollection = database.GetCollection<GameConfig>(GameConfigCollectionName);
        }
    }
}