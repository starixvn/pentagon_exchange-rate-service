using System;

namespace ExchangeRate.Providers
{
    public record ExchangeRateRecord
    {
        public DateTime ExchangeDate { get; init; }

        public string Code { get; init; }

        public string BaseCode { get; init; }

        public decimal BaseRate { get; init; }
    }
}