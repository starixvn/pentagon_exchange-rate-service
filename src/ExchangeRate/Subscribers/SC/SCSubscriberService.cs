namespace ExchangeRate.Subscribers.SC
{
    public interface ISCSubscriberService : ISubscriberService
    {
    }

    public class SCSubscriberService : SubscriberService, ISCSubscriberService
    {
        private const string BaseCode = "USD";

        public SCSubscriberService(ISCExchangeDataAccess exchangeDataAccess, ISCGameConfigDataAccess gameConfigDataAccess)
            : base(BaseCode, exchangeDataAccess, gameConfigDataAccess)
        {
        }
    }
}