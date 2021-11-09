using System;

namespace ExchangeRate.Providers
{
    public class ExchangeRate
    {
        public DateTime ExchangeDate { get; set; }

        public string Code { get; set; }

        public char[] USD { get; set; }
    }
}