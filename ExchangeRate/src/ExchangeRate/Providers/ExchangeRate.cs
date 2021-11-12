using System;

namespace ExchangeRate.Providers
{
    public record ExchangeRate
    {
        public DateTime ExchangeDate { get; init; }

        public string Code { get; init; }

        public decimal USD { get; init; }
    }
}