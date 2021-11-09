using ExchangeRate._Shared.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRate._Shared.Middlewares
{
    public static class AppSettingsMiddleware
    {
        public static AppSettings AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings();

            configuration.GetSection("AppSettings").Bind(appSettings);
            services.AddSingleton<IAppSettings>(appSettings);

            return appSettings;
        }
    }
}