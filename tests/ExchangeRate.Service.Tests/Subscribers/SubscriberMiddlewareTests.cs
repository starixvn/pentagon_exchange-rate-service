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

        [Fact]
        public void AddSubcriberServices_Always_AddSubcriberService()
        {
            // Arrange
            var settings = new SubscriberSettings
            {
                SC = new SubcriberDatabase { ConnectionString = "mongodb://127.0.0.1:27017", DatabaseName = "db" },
                VG = new SubcriberDatabase { ConnectionString = "mongodb://127.0.0.1:27017", DatabaseName = "db" }
            };

            var services = new ServiceCollection();
            services.AddMongoDBServices(settings);

            // Act
            services.AddSubcriberServices(settings);
            var provider = services.BuildServiceProvider();
            var scService = provider.GetRequiredService<ISCSubscriberService>();
            var vgService = provider.GetRequiredService<IVGSubscriberService>();

            // Assert
            Assert.NotNull(scService);
            Assert.NotNull(vgService);
        }
    }
}