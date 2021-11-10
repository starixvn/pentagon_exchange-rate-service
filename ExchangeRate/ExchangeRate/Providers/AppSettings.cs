using ExchangeRate.Providers;
using ExchangeRate.Subscribers;

namespace ExchangeRate._Shared.Configurations
{
    public partial interface IAppSettings
    {
        ExchangeProviderSettings ExchangeProviderSettings { get; set; }

        SubcriberSettings SubcriberSettings { get; set; }
    }

    public partial class AppSettings : IAppSettings
    {
        public ExchangeProviderSettings ExchangeProviderSettings { get; set; }

        public SubcriberSettings SubcriberSettings { get; set; }
    }
}