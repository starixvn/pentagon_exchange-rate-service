using ExchangeRate.Subscribers;

namespace ExchangeRate._Shared.Configurations
{
    public partial interface IAppSettings
    {
        SubcriberSettings SubcriberSettings { get; set; }
    }

    public partial class AppSettings : IAppSettings
    {
        public SubcriberSettings SubcriberSettings { get; set; }
    }
}