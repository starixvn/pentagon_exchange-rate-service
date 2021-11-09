using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate.Providers
{
    public static class ProviderMiddleware
    {
        public static void AddProviderServices(this IServiceCollection services, ExchangeProviderSettings providerServiceSettings)
        {
            services.AddSingleton(providerServiceSettings);
            services.AddSingleton<DapperContext>();
            services.AddScoped<IExchangeProviderService, ExchangeProviderService>();
        }
    }
}