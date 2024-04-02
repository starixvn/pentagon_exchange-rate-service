using Sentry;

namespace ExchangeRate.Subscribers.SN
{
    public interface ISNSubscriberService : ISubscriberService
    {
    }

    public class SNSubscriberService : SubscriberService, ISNSubscriberService
    {
        private const string BaseCode = "USD";

        public SNSubscriberService(ISNExchangeDataAccess exchangeDataAccess, ISNGameConfigDataAccess gameConfigDataAccess, ISentryClient sentryClient)
            : base(BaseCode, exchangeDataAccess, gameConfigDataAccess, sentryClient)
        {
        }
    }
}