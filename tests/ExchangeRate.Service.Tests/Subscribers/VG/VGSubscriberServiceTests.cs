using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using ExchangeRate.Subscribers.VG;
using NSubstitute;
using Xunit;

namespace ExchangeRate.Service.Tests.Subscribers.VG
{
    public class VGSubscriberServiceTests
    {
        private readonly IVGExchangeDataAccess exchangeDataAccess;
        private readonly IVGGameConfigDataAccess gameConfigDataAccess;

        private readonly VGSubcriberService subscriberService;

        public VGSubscriberServiceTests()
        {
            exchangeDataAccess = Substitute.For<IVGExchangeDataAccess>();
            gameConfigDataAccess = Substitute.For<IVGGameConfigDataAccess>();

            subscriberService = new VGSubcriberService(exchangeDataAccess, gameConfigDataAccess);
        }

        [Theory, AutoData]
        public async Task InsertExchangeRates_Success_InjectCorrectCalls(string source, IEnumerable<ExchangeRateRecord> exchangeRates)
        {
            // Arrange

            // Act
            await subscriberService.InsertExchangeRates(source, exchangeRates);

            // Assert
            await exchangeDataAccess.Received().Insert(Arg.Any<ExchangeRateLog>());
            await gameConfigDataAccess.Received().UpdateReloadExchange();
        }
    }
}