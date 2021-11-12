using System.Threading.Tasks;
using Coravel.Events.Interfaces;
using ExchangeRate.Events;
using ExchangeRate.Subscribers.SC;
using ExchangeRate.Subscribers.VG;

namespace ExchangeRate.Subscribers
{
    public class ExchangeListener : IListener<ExchangeRateReceived>
    {
        private readonly ISCSubcriberService SCService;
        private readonly IVGSubcriberService VGService;

        public ExchangeListener(ISCSubcriberService scService, IVGSubcriberService vgService)
        {
            SCService = scService;
            VGService = vgService;
        }

        public async Task HandleAsync(ExchangeRateReceived broadcasted)
        {
            await SCService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            await VGService.InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
        }
    }
}