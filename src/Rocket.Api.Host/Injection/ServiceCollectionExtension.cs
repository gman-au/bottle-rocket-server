using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using Rocket.Api.Host.Exceptions;
using Rocket.Api.Host.Notifiers;
using Rocket.Api.Host.Prepopulation;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Utils;
using Rocket.Domain.Workflows;
using Rocket.Infrastructure;
using Rocket.Infrastructure.Blob.Local;
using Rocket.Infrastructure.Blob.Local.Options;
using Rocket.Infrastructure.Db.Mongo;
using Rocket.Infrastructure.Db.Mongo.Options;
using Rocket.Infrastructure.Detection;
using Rocket.Infrastructure.Hashing;
using Rocket.Infrastructure.Json;
using Rocket.Infrastructure.Mapping;
using Rocket.Infrastructure.QrCode;
using Rocket.Infrastructure.Thumbnails;
using Rocket.Interfaces;
using Rocket.Jobs.Service;
using Rocket.Localization;

namespace Rocket.Api.Host.Injection
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterBsonDomainMappings(
            this IServiceCollection services
        )
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseConnector)))
            {
                BsonClassMap.RegisterClassMap<BaseConnector>(
                    cm =>
                    {
                        cm.AutoMap();
                        cm.SetIsRootClass(true);
                    }
                );
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseWorkflowStep)))
            {
                BsonClassMap.RegisterClassMap<BaseWorkflowStep>(
                    cm =>
                    {
                        cm.AutoMap();
                        cm.SetIsRootClass(true);
                    }
                );
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseExecutionStep)))
            {
                BsonClassMap.RegisterClassMap<BaseExecutionStep>(
                    cm =>
                    {
                        cm.AutoMap();
                        cm.SetIsRootClass(true);
                    }
                );
            }

            var provider =
                services
                    .BuildServiceProvider();

            var bsonMappers =
                provider
                    .GetServices<IBsonMapper>();

            foreach (var bsonMapper in bsonMappers)
            {
                bsonMapper
                    .MapApplicableBsonTypes();
            }
        }

        public static IServiceCollection AddSignalRServerServices(
            this IServiceCollection services,
            IConfigurationRoot configuration
        )
        {
            services
                .AddSignalR();

            var allowedOrigins =
                configuration
                    .GetSection("CorsSettings:AllowedOrigins")
                    .Get<string[]>() ?? [];

            services
                .AddCors(
                    options =>
                    {
                        options
                            .AddPolicy(
                                DomainConstants.BlazorAppCorsPolicy,
                                policy =>
                                {
                                    policy
                                        .WithOrigins(allowedOrigins)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowCredentials();
                                }
                            );
                    }
                );

            services
                .AddSingleton<ICaptureNotifier, CaptureNotifier>();

            return services;
        }

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
                .AddTransient<IDatabasePrepopulator, DatabasePrepopulator>()
                .AddTransient<IWorkflowStepValidator, WorkflowStepValidator>()
                .AddTransient<IEmailAddressValidator, EmailAddressValidator>()
                .AddTransient<IPasswordHasher, PasswordHasher>()
                .AddTransient<IExecutionScheduler, ExecutionScheduler>()
                .AddTransient<ISymbolDetector, SymbolDetector>()
                .AddTransient<IWorkflowDetector, WorkflowDetector>()
                .AddTransient<IQrCodeGenerator, QrCodeGenerator>()
                .AddTransient<IHostResolver, HostResolver>()
                .AddTransient<IActiveAdminChecker, ActiveAdminChecker>()
                .AddTransient<IConnectorScrubber, ConnectorScrubber>()
                .AddTransient<IThumbnailer, Thumbnailer>()
                .AddTransient<IUserManager, UserManager>()
                .AddTransient<ISchemaDictionary, SchemaDictionary>()
                .AddTransient<ISchemaResponseBuilder, SchemaResponseBuilder>()
                .AddTransient<ISchemaGenerator, SchemaGenerator>()
                .AddTransient<IImageBase64Converter, ImageBase64Converter>()
                .AddTransient<IExecutionWorkflowValidator, ExecutionWorkflowValidator>()
                .AddTransient<IWorkflowStepModelMapperRegistry, WorkflowStepModelMapperRegistry>()
                .AddTransient<IExecutionStepModelMapperRegistry, ExecutionStepModelMapperRegistry>()
                .AddTransient<IConnectorModelMapperRegistry, ConnectorModelMapperRegistry>()
                .AddTransient<IStepModelClonerRegistry, StepModelClonerRegistry>()
                .AddTransient<IObfuscator, Obfuscator>()
                .AddTransient<IFileRetitler, SafeFileRetitler>()
                .AddTransient<IWorkflowCloner, WorkflowCloner>()
                .AddSingleton<IDashboardSnapshotProvider, DashboardSnapshotProvider>();

            if (environment.IsDevelopment())
                services
                    .AddTransient<IPasswordGenerator, StaticInsecurePasswordGenerator>();
            else
                services
                    .AddTransient<IPasswordGenerator, RandomPasswordGenerator>();

            services
                .AddSingleton<IJsonResolverInstanceProvider, RocketJsonResolverInstanceProvider>();

            services
                .AddMemoryCache();

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
                .AddTransient<IConnectorRepository, MongoDbConnectorRepository>()
                .AddTransient<IWorkflowRepository, MongoDbWorkflowRepository>()
                .AddTransient<IWorkflowStepRepository, MongoDbWorkflowStepRepository>()
                .AddTransient<IExecutionRepository, MongoDbExecutionRepository>()
                .AddTransient<IRocketbookPageTemplateRepository, MongoDbRocketbookPageTemplateRepository>()
                .AddTransient<IGlobalSettingsRepository, MongoDbGlobalSettingsRepository>()
                .AddTransient<IUserRepository, MongoDbUserRepository>();

            return services;
        }

        public static IServiceCollection AddWorkflowBackgroundJob(
            this IServiceCollection services
        )
        {
            services.AddSingleton<ICaptureSweeper, CaptureSweeper>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddSingleton<IWorkflowExecutionManager, WorkflowExecutionManager>();
            services.AddSingleton<IGlobalSettingsChangedSignal, GlobalSettingsChangedSignal>();
            services.AddTransient<IWorkflowExecutionContext, WorkflowExecutionContext>();
            services.AddHostedService<QueuedHostedService>();
            services.AddHostedService<ScanSweeperHostedService>();

            return services;
        }

        public static IServiceCollection AddJsonSupport(
            this IServiceCollection services
        )
        {
            services
                .AddSingleton<CreateWorkflowStepRequestConverter, CreateWorkflowStepRequestConverter>()
                .AddSingleton<UpdateWorkflowStepRequestConverter, UpdateWorkflowStepRequestConverter>()
                .AddSingleton<CreateConnectorRequestConverter, CreateConnectorRequestConverter>();

            return services;
        }

        public static IServiceCollection AddLocalizationServices(
            this IServiceCollection services
        )
        {
            services
                .AddLocalization(
                    options =>
                    {
                        options.ResourcesPath = "";
                    }
                );

            return services;
        }
    }
}