namespace ExchangeRate.Subscribers.VG
{
    public interface IVGSubscriberService : ISubscriberService
    {
    }

    public class VGSubcriberService : SubscriberService, IVGSubscriberService
    {
        private const string BaseCode = "VND";

        public VGSubcriberService(IVGExchangeDataAccess exchangeDataAccess, IVGGameConfigDataAccess gameConfigDataAccess)
            : base(BaseCode, exchangeDataAccess, gameConfigDataAccess)
        {
        }
    }
}