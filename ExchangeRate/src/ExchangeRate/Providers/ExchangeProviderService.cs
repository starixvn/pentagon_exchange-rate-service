using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace ExchangeRate.Providers
{
    public interface IExchangeProviderService
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRate();
    }

    public class ExchangeProviderService : IExchangeProviderService
    {
        private readonly ExchangeProviderSettings settings;
        private readonly IDapperContext context;

        public ExchangeProviderService(ExchangeProviderSettings settings, IDapperContext context)
        {
            this.settings = settings;
            this.context = context;
        }

        public Task<IEnumerable<ExchangeRate>> GetExchangeRate()
        {
            var connection = context.CreateConnection();

            return connection.QueryAsync<ExchangeRate>
                (settings.ExchangeStoredProcedure, null, commandType: CommandType.StoredProcedure);
        }
    }
}