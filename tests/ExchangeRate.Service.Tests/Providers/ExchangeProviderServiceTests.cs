using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExchangeRate.Providers;
using NSubstitute;
using Xunit;

namespace ExchangeRate.Service.Tests.Providers
{
    public class ExchangeProviderServiceTests
    {
        private readonly IDapperContext context;

        public ExchangeProviderServiceTests()
        {
            context = Substitute.For<IDapperContext>();
        }

        [Theory, AutoData]
        public async Task GetExchangeRate_Success_ReturnsEmptyResult(ExchangeProviderSettings settings)
        {
            // Arrange
            context.QueryAsync<ExchangeRateRecord>(settings.ExchangeStoredProcedure, null, CommandType.StoredProcedure)
                .Returns(new List<ExchangeRateRecord>());

            var service = new ExchangeProviderService(settings, context);

            // Act
            var result = await service.GetExchangeRate();

            // Assert
            Assert.Empty(result);
            await context.Received().QueryAsync<ExchangeRateRecord>(settings.ExchangeStoredProcedure, null, CommandType.StoredProcedure);
        }

        [Theory, AutoData]
        public async Task GetExchangeRate_Success_ReturnsResults(ExchangeProviderSettings settings, List<ExchangeRateRecord> exchangeRates)
        {
            // Arrange
            context.QueryAsync<ExchangeRateRecord>(settings.ExchangeStoredProcedure, null, CommandType.StoredProcedure)
                .Returns(exchangeRates);

            var service = new ExchangeProviderService(settings, context);

            // Act
            var result = await service.GetExchangeRate();

            // Assert
            Assert.Equal(exchangeRates, result);
            await context.Received().QueryAsync<ExchangeRateRecord>(settings.ExchangeStoredProcedure, null, CommandType.StoredProcedure);
        }
    }
}