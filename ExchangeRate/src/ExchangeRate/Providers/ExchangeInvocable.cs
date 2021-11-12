using System;
using System.Threading.Tasks;
using Coravel.Events.Interfaces;
using Coravel.Invocable;
using ExchangeRate.Events;

namespace ExchangeRate.Providers
{
    public class ExchangeInvocable : IInvocable
    {
        private const string Source = "OW";
        private readonly IExchangeProviderService exchangeProviderService;
        private readonly IDispatcher dispatcher;

        public ExchangeInvocable(IExchangeProviderService exchangeProviderService, IDispatcher dispatcher)
        {
            this.exchangeProviderService = exchangeProviderService;
            this.dispatcher = dispatcher;
        }

        public async Task Invoke()
        {
            Console.WriteLine($"{DateTime.Now} - Getting exchange data!");

            var exchangeRates = await exchangeProviderService.GetExchangeRate();

            await dispatcher.Broadcast(new ExchangeRateReceived(Source, exchangeRates));
        }
    }
}