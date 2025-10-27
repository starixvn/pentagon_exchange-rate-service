using Coravel;
using Coravel.Events.Interfaces;
using ExchangeRate._Shared.Middlewares;
using ExchangeRate.Events;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;
using System;

namespace ExchangeRate
{
    public static class Program
    {
        private static string CronSettings = "";

        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            SentrySdk.CaptureMessage($"{DateTime.Now} - Start ExchangeRate", SentryLevel.Info);

            host.Services.UseScheduler(scheduler =>
            {
                scheduler.Schedule<ExchangeInvocable>().Cron(CronSettings);
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
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((hostContext, services) =>
                    {
                        services.AddHealthChecks();
                        services.AddRouting();
                    });
                    
                    webBuilder.Configure(app =>
                    {
                        // Health check endpoint for K8s
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapHealthChecks("/");
                        });
                    });
                    
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        var appSettings = context.Configuration.GetSection("AppSettings").Get<ExchangeRate._Shared.Configurations.AppSettings>();
                        options.ListenAnyIP(appSettings.Port);
                    });
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var appSettings = services.AddSettings(hostContext.Configuration);
                    CronSettings = appSettings.Cron;

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