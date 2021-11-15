using System.Collections.Generic;
using Coravel.Events.Interfaces;

namespace ExchangeRate.Events
{
    public record ExchangeRateReceived : IEvent
    {
        public ExchangeRateReceived(string source, IEnumerable<Providers.ExchangeRateRecord> exchangeRate)
        {
            Source = source;
            ExchangeRate = exchangeRate;
        }

        public string Source { get; init; }

        public IEnumerable<Providers.ExchangeRateRecord> ExchangeRate { get; init; }
    }
}