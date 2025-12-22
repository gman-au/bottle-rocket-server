using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Host.Exceptions;
using Rocket.Infrastructure.MongoDb;
using Rocket.Infrastructure.MongoDb.Options;

namespace Rocket.Api.Host.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBottleRocketApiServices(this IServiceCollection services)
        {
            services
                .AddTransient<IRocketExceptionWrapper, RocketExceptionWrapper>();

            return services;
        }
        
        public static IServiceCollection AddMongoDbServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .Configure<MongoDbConfigurationOptions>(
                    configuration
                        .GetSection(nameof(MongoDbConfigurationOptions))
                );

            services
                .AddSingleton<IMongoDbClient, MongoDbClient>();

            return services;
        }
    }
}