using Coravel;
using Coravel.Events.Interfaces;
using ExchangeRate._Shared.Middlewares;
using ExchangeRate.Events;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sentry;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public static class Program
    {
        private static string CronSettings = "";
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions 
        { 
            WriteIndented = true 
        };

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
                    
                    webBuilder.Configure((context, app) =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapHealthChecks("/", new HealthCheckOptions
                            {
                                ResponseWriter = async (httpContext, healthReport) =>
                                {
                                    httpContext.Response.ContentType = "application/json";
                                    
                                    var response = new
                                    {
                                        status = healthReport.Status.ToString(),
                                        serviceName = "ExchangeRateService",
                                        environment = context.Configuration["ENV"] ?? "Unknown",
                                        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                        version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0",
                                        checks = healthReport.Entries.Select(e => new
                                        {
                                            name = e.Key,
                                            status = e.Value.Status.ToString(),
                                            description = e.Value.Description,
                                            duration = e.Value.Duration.TotalMilliseconds
                                        })
                                    };
                                    
                                    await httpContext.Response.WriteAsync(
                                        JsonSerializer.Serialize(response, JsonOptions));
                                }
                            });
                        });
                    });
                    
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        var appSettings = context.Configuration.GetSection("AppSettings").Get<ExchangeRate._Shared.Configurations.AppSettings>();
                        
                        options.ListenAnyIP(appSettings.Port);
                    });
                    
                    // Suppress default URL configuration to avoid conflicts
                    webBuilder.UseUrls();
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