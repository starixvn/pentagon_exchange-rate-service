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

        public ExchangeListener(IAppSettings appSettings, ISCSubscriberService scService = null, IVGSubscriberService vgService = null, ISNSubscriberService snService = null)
        {
            this.appSettings = appSettings;
            SCService = scService;
            VGService = vgService;
            SNService = snService;
        }

        public async Task HandleAsync(ExchangeRateReceived broadcasted)
        {
            if (SCService != null && appSettings.SubcriberSettings?.SC != null && appSettings.SubcriberSettings.SC.Enabled)
            {
                await SCService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            }

            if (VGService != null && appSettings.SubcriberSettings?.VG != null && appSettings.SubcriberSettings.VG.Enabled)
            {
                await VGService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            }

            if (SNService != null && appSettings.SubcriberSettings?.SN != null && appSettings.SubcriberSettings.SN.Enabled)
            {
                await SNService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            }
        }
    }
}