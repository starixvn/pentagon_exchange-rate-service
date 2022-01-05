using Sentry;

namespace ExchangeRate.Subscribers.SC
{
    public interface ISCSubscriberService : ISubscriberService
    {
    }

    public class SCSubscriberService : SubscriberService, ISCSubscriberService
    {
        private const string BaseCode = "USD";

        public SCSubscriberService(ISCExchangeDataAccess exchangeDataAccess, ISCGameConfigDataAccess gameConfigDataAccess, ISentryClient sentryClient)
            : base(BaseCode, exchangeDataAccess, gameConfigDataAccess, sentryClient)
        {
        }
    }
}