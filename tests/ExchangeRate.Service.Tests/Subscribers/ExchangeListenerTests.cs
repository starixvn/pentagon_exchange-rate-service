using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExchangeRate.Events;
using ExchangeRate.Subscribers;
using ExchangeRate.Subscribers.SC;
using ExchangeRate.Subscribers.VG;
using NSubstitute;
using Xunit;

namespace ExchangeRate.Service.Tests.Subscribers
{
    public class ExchangeListenerTests
    {
        private readonly ISCSubcriberService SCService;
        private readonly IVGSubcriberService VGService;

        private readonly ExchangeListener listener;

        public ExchangeListenerTests()
        {
            SCService = Substitute.For<ISCSubcriberService>();
            VGService = Substitute.For<IVGSubcriberService>();

            listener = new ExchangeListener(SCService, VGService);
        }

        [Theory, AutoData]
        public async Task HandleAsync_Success_ShouldInsertExchangeRates(ExchangeRateReceived broadcasted)
        {
            // Arrange

            // Act
            await listener.HandleAsync(broadcasted);

            // Assert
            await SCService.Received().InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            await VGService.Received().InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
        }
    }
}