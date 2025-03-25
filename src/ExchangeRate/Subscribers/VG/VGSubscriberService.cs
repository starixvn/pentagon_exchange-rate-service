using Sentry;

namespace ExchangeRate.Subscribers.VG
{
    public interface IVGSubscriberService : ISubscriberService
    {
    }

    public class VGSubcriberService : SubscriberService, IVGSubscriberService
    {
        private const string BaseCode = "USD";
        private const string System = "VG";

        public VGSubcriberService(IVGExchangeDataAccess exchangeDataAccess, IVGGameConfigDataAccess gameConfigDataAccess, ISentryClient sentryClient)
            : base(BaseCode, exchangeDataAccess, gameConfigDataAccess, sentryClient, System)
        {
        }
    }
}