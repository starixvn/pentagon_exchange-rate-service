namespace ExchangeRate.Subscribers.VG
{
    public interface IVGSubscriberService : ISubscriberService
    {
    }

    public class VGSubcriberService : SubscriberService, IVGSubscriberService
    {
        public VGSubcriberService(IVGExchangeDataAccess exchangeDataAccess, IVGGameConfigDataAccess gameConfigDataAccess)
            : base(exchangeDataAccess, gameConfigDataAccess)
        {
        }
    }
}