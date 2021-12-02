using Coravel;
using Coravel.Events.Interfaces;
using ExchangeRate._Shared.Middlewares;
using ExchangeRate.Events;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;
using TinyHealthCheck;

namespace ExchangeRate
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<ExchangeInvocable>().Cron("00 02,14 * * *");
            }).OnError((exception) =>
                  SentrySdk.CaptureException(exception)
            );

            IEventRegistration registration = host.Services.ConfigureEvents();

            registration
                .Register<ExchangeRateReceived>()
                .Subscribe<ExchangeListener>();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var appSettings = services.AddSettings(hostContext.Configuration);
                    services.AddBasicTinyHealthCheck(config =>
                    {
                        config.Port = appSettings.Port;
                        config.UrlPath = "/";

                        return config;
                    });

                    services.AddSchedulers();
                    services.AddEventListeners();

                    services.AddMongoDBServices(appSettings.SubcriberSettings);

                    services.AddProviderServices(appSettings.ExchangeProviderSettings);
                    services.AddSubcriberServices(appSettings.SubcriberSettings);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddSentry(opt => opt.Environment = hostingContext.Configuration["ENV"]);
                });
    }
}