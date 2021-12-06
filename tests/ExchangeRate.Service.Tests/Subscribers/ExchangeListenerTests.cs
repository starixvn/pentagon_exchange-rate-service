using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExchangeRate._Shared.Configurations;
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
        private readonly ISCSubscriberService SCService;
        private readonly IVGSubscriberService VGService;
        private readonly IAppSettings appSettings;

        private readonly ExchangeListener listener;

        public ExchangeListenerTests()
        {
            SCService = Substitute.For<ISCSubscriberService>();
            VGService = Substitute.For<IVGSubscriberService>();
            appSettings = Substitute.For<IAppSettings>();

            listener = new ExchangeListener(SCService, VGService, appSettings);
        }

        [Theory, AutoData]
        public async Task HandleAsync_Success_ShouldInsertExchangeRates(ExchangeRateReceived broadcasted, SubscriberSettings subscriberSettings)
        {
            // Arrange
            appSettings.SubcriberSettings = subscriberSettings;
            appSettings.SubcriberSettings.VG.Enabled = true;

            // Act
            await listener.HandleAsync(broadcasted);

            // Assert
            await SCService.Received().InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            await VGService.Received().InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
        }

        [Theory, AutoData]
        public async Task HandleAsync_DisabledSubcriber_ShouldNotInsertExchangeRates(ExchangeRateReceived broadcasted, SubscriberSettings subscriberSettings)
        {
            // Arrange
            appSettings.SubcriberSettings = subscriberSettings;
            appSettings.SubcriberSettings.VG.Enabled = false;

            // Act
            await listener.HandleAsync(broadcasted);

            // Assert
            await SCService.Received().InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
            await VGService.DidNotReceive().InsertExchangeRates(broadcasted.Source, broadcasted.ExchangeRate);
        }
    }
}