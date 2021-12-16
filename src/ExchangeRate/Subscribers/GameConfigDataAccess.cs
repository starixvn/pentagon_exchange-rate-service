using System.Threading.Tasks;
using MongoDB.Driver;

namespace ExchangeRate.Subscribers
{
    public interface IGameConfigDataAccess
    {
        Task UpdateReloadExchange();
    }

    public class GameConfigDataAccess : IGameConfigDataAccess
    {
        protected IMongoCollection<GameConfig> gameConfigCollection;

        protected GameConfigDataAccess()
        {
        }

        public Task UpdateReloadExchange()
        {
            var update = Builders<GameConfig>.Update
                    .Set(field => field.ReloadExchange, true);

            return gameConfigCollection.UpdateManyAsync(config => true, update);
        }
    }
}