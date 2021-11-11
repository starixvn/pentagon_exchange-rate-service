using ExchangeRate.Subscribers.SC;
using ExchangeRate.Subscribers.VG;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace ExchangeRate.Subscribers
{
    public static class SubcriberMiddleware
    {
        public static void AddSubcriberServices(this IServiceCollection services, SubcriberSettings settings)
        {
            services.AddSingleton(settings);

            services.AddScoped<ISCExchangeDataAccess, SCExchangeDataAccess>();
            services.AddScoped<ISCSubcriberService, SCSubcriberService>();

            services.AddScoped<IVGExchangeDataAccess, VGExchangeDataAccess>();
            services.AddScoped<IVGSubcriberService, VGSubcriberService>();
        }

        public static void AddMongoDBServices(this IServiceCollection services, SubcriberSettings settings)
        {
            var camelIgnoringExtraElementConventions = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            };

            ConventionRegistry.Register(nameof(camelIgnoringExtraElementConventions), camelIgnoringExtraElementConventions, t => true);
            BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));

            services.AddSingleton<ISCMongoClient>(new SCMongoClient(settings.SC));
            services.AddSingleton<IVGMongoClient>(new VGMongoClient(settings.VG));
        }
    }
}