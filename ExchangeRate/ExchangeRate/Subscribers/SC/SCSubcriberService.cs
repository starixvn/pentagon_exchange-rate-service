namespace ExchangeRate.Subscribers.SC
{
    public interface ISCSubcriberService : ISubcriberService
    {
    }

    public class SCSubcriberService : SubcriberService, ISCSubcriberService
    {
        public SCSubcriberService(ISCExchangeDataAccess exchangeDataAccess, ISCGameConfigDataAccess gameConfigDataAccess)
            : base(exchangeDataAccess, gameConfigDataAccess)
        {
        }
    }
}