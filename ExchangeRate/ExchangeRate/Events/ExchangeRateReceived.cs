using System.Collections.Generic;
using Coravel.Events.Interfaces;

namespace ExchangeRate.Events
{
    public class ExchangeRateReceived : IEvent
    {
        public ExchangeRateReceived(string source, IEnumerable<Providers.ExchangeRate> exchangeRate)
        {
            Source = source;
            ExchangeRate = exchangeRate;
        }

        public string Source { get; }

        public IEnumerable<Providers.ExchangeRate> ExchangeRate { get; }
    }
}