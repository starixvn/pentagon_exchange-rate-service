using AutoFixture.Xunit2;
using ExchangeRate.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExchangeRate.Service.Tests.Providers
{
    public class ProviderMiddlewareTests
    {
        [Theory, AutoData]
        public void AddProviderServices_Always_AddProviderSettings(ExchangeProviderSettings providerSettings)
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddProviderServices(providerSettings);
            var provider = services.BuildServiceProvider();
            var result = provider.GetRequiredService<ExchangeProviderSettings>();

            // Assert
            Assert.Equal(providerSettings, result);
        }
    }
}