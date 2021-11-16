namespace ExchangeRate.Subscribers.SC
{
    public interface ISCSubscriberService : ISubscriberService
    {
    }

    public class SCSubscriberService : SubscriberService, ISCSubscriberService
    {
        public SCSubscriberService(ISCExchangeDataAccess exchangeDataAccess, ISCGameConfigDataAccess gameConfigDataAccess)
            : base(exchangeDataAccess, gameConfigDataAccess)
        {
        }
    }
}