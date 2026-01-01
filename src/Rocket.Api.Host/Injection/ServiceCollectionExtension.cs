using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Api.Host.Exceptions;
using Rocket.Infrastructure;
using Rocket.Infrastructure.Blob.Local;
using Rocket.Infrastructure.Blob.Local.Options;
using Rocket.Infrastructure.Db.Mongo;
using Rocket.Infrastructure.Db.Mongo.Options;
using Rocket.Infrastructure.Hashing;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBottleRocketApiServices(
            this IServiceCollection services,
            IWebHostEnvironment environment
        )
        {
            services
                .AddTransient<IRocketExceptionWrapper, RocketExceptionWrapper>()
                .AddTransient<IScannedImageHandler, ScannedImageHandler>()
                .AddTransient<IAuthenticator, Authenticator>()
                .AddTransient<ISha256Calculator, Sha256Calculator>()
                .AddTransient<IStartupInitialization, StartupInitialization>()
                .AddTransient<IPasswordHasher, PasswordHasher>()
                .AddTransient<IUserManager, UserManager>();

            if (environment.IsDevelopment())
                services
                    .AddTransient<IPasswordGenerator, StaticInsecurePasswordGenerator>();
            else
                services
                    .AddTransient<IPasswordGenerator, RandomPasswordGenerator>();

            return services;
        }

        public static IServiceCollection AddLocalFileBlobStore(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .Configure<LocalBlobConfigurationOptions>(
                    configuration
                        .GetSection(nameof(LocalBlobConfigurationOptions))
                );

            services
                .AddTransient<IBlobStore, LocalBlobStore>();

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

            services
                .AddTransient<IScannedImageRepository, MongoDbScannedImageRepository>()
                .AddTransient<IUserRepository, MongoDbUserRepository>();

            return services;
        }
    }
}