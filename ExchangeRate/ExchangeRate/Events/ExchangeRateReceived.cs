using System.Collections.Generic;
using Coravel.Events.Interfaces;

namespace ExchangeRate.Events
{
    public class ExchangeRateReceived : IEvent
    {
        public IEnumerable<Providers.ExchangeRate> ExchangeRate { get; set; }

        public ExchangeRateReceived(IEnumerable<Providers.ExchangeRate> exchangeRate)
        {
            ExchangeRate = exchangeRate;
        }
    }
}