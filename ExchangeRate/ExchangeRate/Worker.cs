using Microsoft.Extensions.Hosting;
using Sentry;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public class Worker : BackgroundService
    {
        private readonly ISentryClient sentryClient;

        public Worker(ISentryClient sentryClient)
        {
            this.sentryClient = sentryClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                sentryClient.CaptureMessage($"Worker running at: {DateTimeOffset.Now}");
                await Task.Delay(1000000, stoppingToken);
            }
        }
    }
}