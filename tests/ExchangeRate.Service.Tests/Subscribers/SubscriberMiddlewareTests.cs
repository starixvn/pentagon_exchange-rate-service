using AutoFixture.Xunit2;
using ExchangeRate.Subscribers;
using ExchangeRate.Subscribers.SC;
using ExchangeRate.Subscribers.VG;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExchangeRate.Service.Tests.Subscribers
{
    public class SubscriberMiddlewareTests
    {
        [Theory, AutoData]
        public void AddSubcriberServices_Always_AddSubcriberSettings(SubscriberSettings settings)
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddSubcriberServices(settings);
            var provider = services.BuildServiceProvider();
            var result = provider.GetRequiredService<SubscriberSettings>();

            // Assert
            Assert.Equal(settings, result);
        }
    }
}