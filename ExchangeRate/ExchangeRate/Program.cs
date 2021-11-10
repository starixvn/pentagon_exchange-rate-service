using Coravel;
using Coravel.Events.Interfaces;
using ExchangeRate._Shared.Middlewares;
using ExchangeRate.Events;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;

namespace ExchangeRate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            host.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<ExchangeInvocable>().EveryMinute();
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

                    services.AddHostedService<Worker>();
                    services.AddScheduler();
                    services.AddEvents();

                    services.AddTransient<ExchangeInvocable>();
                    services.AddTransient<ExchangeListener>();
                    services.AddProviderServices(appSettings.ExchangeProviderSettings);
                    services.AddSubcriberServices(appSettings.SubcriberSettings);
                    services.AddMongoDBServices(appSettings.SubcriberSettings);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddSentry(opt => opt.Environment = hostingContext.Configuration["ENV"]);
                });
    }
}