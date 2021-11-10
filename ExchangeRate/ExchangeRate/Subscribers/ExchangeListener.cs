using System.Threading.Tasks;
using Coravel.Events.Interfaces;
using ExchangeRate.Events;

namespace ExchangeRate.Subscribers
{
    public class ExchangeListener : IListener<ExchangeRateReceived>
    {
        private readonly ISubcriberService subscriberService;

        public ExchangeListener(ISubcriberService subscriberService)
        {
            this.subscriberService = subscriberService;
        }

        public Task HandleAsync(ExchangeRateReceived broadcasted)
        => subscriberService.InsertExchangeRates(broadcasted.ExchangeRate);
    }
}