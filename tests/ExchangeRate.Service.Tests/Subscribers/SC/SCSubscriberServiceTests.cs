using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using ExchangeRate.Subscribers.SC;
using NSubstitute;
using Xunit;

namespace ExchangeRate.Service.Tests.Subscribers.SC
{
    public class SCSubscriberServiceTests
    {
        private readonly ISCExchangeDataAccess exchangeDataAccess;
        private readonly ISCGameConfigDataAccess gameConfigDataAccess;

        private readonly SCSubscriberService subscriberService;

        public SCSubscriberServiceTests()
        {
            exchangeDataAccess = Substitute.For<ISCExchangeDataAccess>();
            gameConfigDataAccess = Substitute.For<ISCGameConfigDataAccess>();

            subscriberService = new SCSubscriberService(exchangeDataAccess, gameConfigDataAccess);
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

        [Theory, AutoData]
        public async Task InsertExchangeRatesWithLatestId_Success_InjectCorrectCalls(string source, IEnumerable<ExchangeRateRecord> exchangeRates)
        {
            // Arrange
            exchangeDataAccess.GetLastExchange().Returns(new ExchangeRateLog { ExId = 1 });

            // Act
            await subscriberService.InsertExchangeRates(source, exchangeRates);

            // Assert
            await exchangeDataAccess.Received().Insert(Arg.Is<ExchangeRateLog>( x => x.ExId == 2));
            await gameConfigDataAccess.Received().UpdateReloadExchange();
        }
    }
}