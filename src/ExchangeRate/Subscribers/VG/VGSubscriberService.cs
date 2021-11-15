namespace ExchangeRate.Subscribers.VG
{
    public interface IVGSubcriberService : ISubcriberService
    {
    }

    public class VGSubcriberService : SubcriberService, IVGSubcriberService
    {
        public VGSubcriberService(IVGExchangeDataAccess exchangeDataAccess, IVGGameConfigDataAccess gameConfigDataAccess)
            : base(exchangeDataAccess, gameConfigDataAccess)
        {
        }
    }
}