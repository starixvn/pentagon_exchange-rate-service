using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using ExchangeRate._Shared.Configurations;
using ExchangeRate.Events;
using ExchangeRate.Providers;
using ExchangeRate.Subscribers;
using ExchangeRate.Subscribers.SC;
using ExchangeRate.Subscribers.SN;
using ExchangeRate.Subscribers.VG;
using NSubstitute;
using Xunit;

namespace ExchangeRate.Service.Tests.Subscribers
{
    public class ExchangeListenerTests
    {
        private readonly ISCSubscriberService scService;
        private readonly IVGSubscriberService vgService;
        private readonly ISNSubscriberService snService;

        public ExchangeListenerTests()
        {
            scService = Substitute.For<ISCSubscriberService>();
            vgService = Substitute.For<IVGSubscriberService>();
            snService = Substitute.For<ISNSubscriberService>();
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenAllEnabled_InvokesSCService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: true, vgEnabled: true, snEnabled: true);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await scService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenAllEnabled_InvokesVGService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: true, vgEnabled: true, snEnabled: true);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await vgService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenAllEnabled_InvokesSNService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: true, vgEnabled: true, snEnabled: true);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await snService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenOnlySCEnabled_InvokesSCService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: true, vgEnabled: false, snEnabled: false);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await scService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenOnlySCEnabled_DoesNotInvokeVGService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: true, vgEnabled: false, snEnabled: false);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await vgService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenOnlySCEnabled_DoesNotInvokeSNService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: true, vgEnabled: false, snEnabled: false);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await snService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenAllDisabled_DoesNotInvokeSCService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: false, vgEnabled: false, snEnabled: false);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await scService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenAllDisabled_DoesNotInvokeVGService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: false, vgEnabled: false, snEnabled: false);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await vgService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenAllDisabled_DoesNotInvokeSNService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: false, vgEnabled: false, snEnabled: false);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await snService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenSCSettingsNull_DoesNotInvokeSCService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettingsWithSCNull();
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await scService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenSCSettingsNull_InvokesVGService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettingsWithSCNull();
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await vgService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenSCSettingsNull_InvokesSNService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettingsWithSCNull();
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await snService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenSubscriberSettingsNull_DoesNotInvokeSCService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettingsWithNullSubscriberSettings();
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await scService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenSubscriberSettingsNull_DoesNotInvokeVGService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettingsWithNullSubscriberSettings();
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await vgService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenSubscriberSettingsNull_DoesNotInvokeSNService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettingsWithNullSubscriberSettings();
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await snService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenVGAndSNEnabled_DoesNotInvokeSCService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: false, vgEnabled: true, snEnabled: true);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await scService.DidNotReceive().InsertExchangeRates(Arg.Any<string>(), Arg.Any<IEnumerable<ExchangeRateRecord>>());
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenVGAndSNEnabled_InvokesVGService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: false, vgEnabled: true, snEnabled: true);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await vgService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        [Theory, AutoData]
        public async Task HandleAsync_WhenVGAndSNEnabled_InvokesSNService(
            string source,
            List<ExchangeRateRecord> exchangeRates)
        {
            var appSettings = CreateAppSettings(scEnabled: false, vgEnabled: true, snEnabled: true);
            var sut = CreateSut(appSettings);
            var @event = new ExchangeRateReceived(source, exchangeRates);

            await sut.HandleAsync(@event);

            await snService.Received(1).InsertExchangeRates(source, exchangeRates);
        }

        private ExchangeListener CreateSut(IAppSettings appSettings)
        {
            return new ExchangeListener(scService, vgService, snService, appSettings);
        }

        private static IAppSettings CreateAppSettings(bool scEnabled, bool vgEnabled, bool snEnabled)
        {
            var settings = Substitute.For<IAppSettings>();
            settings.SubcriberSettings.Returns(new SubscriberSettings
            {
                SC = new SubcriberDatabase { Enabled = scEnabled },
                VG = new SubcriberDatabase { Enabled = vgEnabled },
                SN = new SubcriberDatabase { Enabled = snEnabled }
            });
            return settings;
        }

        private static IAppSettings CreateAppSettingsWithSCNull()
        {
            var settings = Substitute.For<IAppSettings>();
            settings.SubcriberSettings.Returns(new SubscriberSettings
            {
                SC = null,
                VG = new SubcriberDatabase { Enabled = true },
                SN = new SubcriberDatabase { Enabled = true }
            });
            return settings;
        }

        private static IAppSettings CreateAppSettingsWithNullSubscriberSettings()
        {
            var settings = Substitute.For<IAppSettings>();
            settings.SubcriberSettings.Returns((SubscriberSettings)null);
            return settings;
        }
    }
}
