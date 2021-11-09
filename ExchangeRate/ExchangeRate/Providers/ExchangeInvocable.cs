using Coravel.Invocable;
using System;
using System.Threading.Tasks;

namespace ExchangeRate.Providers
{
    public class ExchangeInvocable : IInvocable
    {
        private readonly IExchangeProviderService exchangeProviderService;

        public ExchangeInvocable(IExchangeProviderService exchangeProviderService) 
        {
            this.exchangeProviderService = exchangeProviderService;
        }

        public async Task Invoke()
        {
            Console.WriteLine($"{DateTime.Now} - Getting exchange data!");

            var exchangeRates = await exchangeProviderService.GetExchangeRate();
        }
    }
}