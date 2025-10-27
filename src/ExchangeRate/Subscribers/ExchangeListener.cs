using System.Threading.Tasks;
using Coravel.Events.Interfaces;
using ExchangeRate._Shared.Configurations;
using ExchangeRate.Events;
using ExchangeRate.Subscribers.SC;
using ExchangeRate.Subscribers.SN;
using ExchangeRate.Subscribers.VG;

namespace ExchangeRate.Subscribers
{
    public class ExchangeListener : IListener<ExchangeRateReceived>
    {
        private readonly ISCSubscriberService SCService;
        private readonly IVGSubscriberService VGService;
        private readonly ISNSubscriberService SNService;
        private readonly IAppSettings appSettings;

        public ExchangeListener(ISCSubscriberService scService, IVGSubscriberService vgService, ISNSubscriberService snService, IAppSettings appSettings)
        {
            SCService = scService;
            VGService = vgService;
            SNService = snService;
            this.appSettings = appSettings;
        }

        public async Task HandleAsync(ExchangeRateReceived broadcasted)
        {
            if (appSettings.SubcriberSettings?.SC != null && appSettings.SubcriberSettings.SC.Enabled)
            {
                await SCService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            }

            if (appSettings.SubcriberSettings?.VG != null && appSettings.SubcriberSettings.VG.Enabled)
            {
                await VGService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            }

            if (appSettings.SubcriberSettings?.SN != null && appSettings.SubcriberSettings.SN.Enabled)
            {
                await SNService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            }
        }
    }
}