using System.Threading.Tasks;
using Coravel.Events.Interfaces;
using ExchangeRate._Shared.Configurations;
using ExchangeRate.Events;
using ExchangeRate.Subscribers.SC;
using ExchangeRate.Subscribers.VG;

namespace ExchangeRate.Subscribers
{
    public class ExchangeListener : IListener<ExchangeRateReceived>
    {
        private readonly ISCSubscriberService SCService;
        private readonly IVGSubscriberService VGService;
        private readonly IAppSettings appSettings;

        public ExchangeListener(ISCSubscriberService scService, IVGSubscriberService vgService, IAppSettings appSettings)
        {
            SCService = scService;
            VGService = vgService;
            this.appSettings = appSettings;
        }

        public async Task HandleAsync(ExchangeRateReceived broadcasted)
        {
            await SCService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            await VGService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
        }
    }
}