using ExchangeRate.Providers;

namespace ExchangeRate._Shared.Configurations
{
    public partial interface IAppSettings
    {
        ExchangeProviderSettings ExchangeProviderSettings { get; set; }
    }

    public partial class AppSettings : IAppSettings
    {
        public ExchangeProviderSettings ExchangeProviderSettings { get; set; }
    }
}