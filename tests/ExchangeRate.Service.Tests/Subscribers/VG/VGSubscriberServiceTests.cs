using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using ExchangeRate.Subscribers.VG;
using NSubstitute;
using Sentry;
using Xunit;

namespace ExchangeRate.Service.Tests.Subscribers.VG
{
    public class VGSubscriberServiceTests
    {
        private readonly IVGExchangeDataAccess exchangeDataAccess;
        private readonly IVGGameConfigDataAccess gameConfigDataAccess;
        private readonly ISentryClient sentryClient;

        private readonly VGSubcriberService subscriberService;

        public VGSubscriberServiceTests()
        {
            exchangeDataAccess = Substitute.For<IVGExchangeDataAccess>();
            gameConfigDataAccess = Substitute.For<IVGGameConfigDataAccess>();
            sentryClient = Substitute.For<ISentryClient>();

            subscriberService = new VGSubcriberService(exchangeDataAccess, gameConfigDataAccess, sentryClient);
        }

        [Theory, AutoData]
        public async Task InsertExchangeRates_SuccessWithoutCodes_NotInjectCorrectCalls(string source, IEnumerable<ExchangeRateRecord> exchangeRates)
        {
            // Arrange

            // Act
            await subscriberService.InsertExchangeRates(source, exchangeRates);

            // Assert
            await exchangeDataAccess.DidNotReceive().Insert(Arg.Any<ExchangeRateLog>());
            await gameConfigDataAccess.DidNotReceive().UpdateReloadExchange();
        }

        [Theory, AutoData]
        public async Task InsertExchangeRates_Success_InjectCorrectCalls(string source)
        {
            // Arrange
            var exchangeRates = new List<ExchangeRateRecord>
            {
                new ExchangeRateRecord{ExchangeDate = DateTime.Now, BaseCode = "USD", BaseRate = 1.1245M, Code = "INH"}
            };

            // Act
            await subscriberService.InsertExchangeRates(source, exchangeRates);

            // Assert
            await exchangeDataAccess.Received().Insert(Arg.Any<ExchangeRateLog>());
            await gameConfigDataAccess.Received().UpdateReloadExchange();
        }
    }
}