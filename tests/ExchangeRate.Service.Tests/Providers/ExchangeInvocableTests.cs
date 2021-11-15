using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Coravel.Events.Interfaces;
using ExchangeRate.Events;
using ExchangeRate.Providers;
using NSubstitute;
using Xunit;

namespace ExchangeRate.Service.Tests.Providers
{
    public class ExchangeInvocableTests
    {
        private readonly IExchangeProviderService exchangeProviderService;
        private readonly IDispatcher dispatcher;

        private readonly ExchangeInvocable exchangeInvocable;

        public ExchangeInvocableTests()
        {
            exchangeProviderService = Substitute.For<IExchangeProviderService>();
            dispatcher = Substitute.For<IDispatcher>();

            exchangeInvocable = new ExchangeInvocable(exchangeProviderService, dispatcher);
        }

        [Theory, AutoData]
        public async Task Invoke_Success_BroadcastExchangeEvent(List<ExchangeRateRecord> exchangeRates)
        {
            // Arrange
            exchangeProviderService.GetExchangeRate().Returns(exchangeRates);

            // Act
            await exchangeInvocable.Invoke();

            // Assert
            await exchangeProviderService.Received().GetExchangeRate();
            await dispatcher.Received().Broadcast(Arg.Any<ExchangeRateReceived>());
        }
    }
}