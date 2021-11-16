using Coravel;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate._Shared.Middlewares
{
    public static class ServiceMiddleware
    {
        public static void AddSchedulers(this IServiceCollection services)
        {
            services.AddScheduler();

            services.AddTransient<ExchangeInvocable>();
        }

        public static void AddEventListeners(this IServiceCollection services)
        {
            services.AddEvents();

            services.AddTransient<ExchangeListener>();
        }
    }
}