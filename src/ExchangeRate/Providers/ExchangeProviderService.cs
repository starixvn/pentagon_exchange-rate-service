using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ExchangeRate.Providers
{
    public interface IExchangeProviderService
    {
        Task<IEnumerable<ExchangeRateRecord>> GetExchangeRate();
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

        public Task<IEnumerable<ExchangeRateRecord>> GetExchangeRate()
        => context.QueryAsync<ExchangeRateRecord>
                (settings.ExchangeStoredProcedure, null, commandType: CommandType.StoredProcedure);
    }
}