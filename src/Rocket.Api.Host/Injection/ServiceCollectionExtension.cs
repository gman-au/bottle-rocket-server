using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Rocket.Api.Host.Exceptions;
using Rocket.Api.Host.Notifiers;
using Rocket.Api.Host.Prepopulation;
using Rocket.Domain.Utils;
using Rocket.Infrastructure;
using Rocket.Infrastructure.Blob.Local;
using Rocket.Infrastructure.Blob.Local.Options;
using Rocket.Infrastructure.Db.Mongo;
using Rocket.Infrastructure.Db.Mongo.Options;
using Rocket.Infrastructure.Detection;
using Rocket.Infrastructure.Hashing;
using Rocket.Infrastructure.Mapping;
using Rocket.Infrastructure.Thumbnails;
using Rocket.Interfaces;
using Rocket.Jobs.Service;

namespace Rocket.Api.Host.Injection
{
    public static class ServiceCollectionExtension
    {
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
                .AddTransient<ISymbolDetector, SymbolDetector>()
                .AddTransient<IActiveAdminChecker, ActiveAdminChecker>()
                .AddTransient<IThumbnailer, Thumbnailer>()
                .AddTransient<IUserManager, UserManager>()
                .AddTransient<IWorkflowStepModelMapperRegistry, WorkflowStepModelMapperRegistry>()
                .AddTransient<IExecutionStepModelMapperRegistry, ExecutionStepModelMapperRegistry>()
                .AddTransient<IStepModelClonerRegistry, StepModelClonerRegistry>()
                .AddTransient<IWorkflowCloner, WorkflowCloner>();

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
                .AddTransient<IConnectorRepository, MongoDbConnectorRepository>()
                .AddTransient<IWorkflowRepository, MongoDbWorkflowRepository>()
                .AddTransient<IWorkflowStepRepository, MongoDbWorkflowStepRepository>()
                .AddTransient<IExecutionRepository, MongoDbExecutionRepository>()
                .AddTransient<IRocketbookPageTemplateRepository, MongoDbRocketbookPageTemplateRepository>()
                .AddTransient<IUserRepository, MongoDbUserRepository>();

            return services;
        }

        public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
        {
            services
                .AddOpenApi(
                    options =>
                    {
                        options.ShouldInclude = _ => true;

                        // Use an Operation Transformer to copy the GroupName into the Tags list
                        options
                            .AddOperationTransformer(
                                (operation, context, _) =>
                                {
                                    // Search for the EndpointGroupNameAttribute in the endpoint metadata
                                    var groupNameMetadata =
                                        context
                                            .Description
                                            .ActionDescriptor
                                            .EndpointMetadata
                                            .OfType<EndpointGroupNameAttribute>()
                                            .FirstOrDefault();

                                    if (groupNameMetadata != null)
                                    {
                                        operation
                                            .Tags
                                            .Clear();

                                        operation
                                            .Tags
                                            .Add(
                                                new OpenApiTag
                                                {
                                                    Name = groupNameMetadata.EndpointGroupName
                                                }
                                            );
                                    }

                                    if (!string.IsNullOrEmpty(operation.Description))
                                    {
                                        operation.Description =
                                            operation
                                                .Description
                                                .Replace(
                                                    "\\n",
                                                    "\n"
                                                );
                                    }

                                    return
                                        Task
                                            .CompletedTask;
                                }
                            );

                        options.AddDocumentTransformer(
                            (document, context, cancellationToken) =>
                            {
                                document.Info.Title = "Bottle Rocket API";
                                document.Info.Version = "##{VERSION_TAG}##";
                                document.Tags = null;

                                return Task.CompletedTask;
                            }
                        );
                    }
                );

            return services;
        }

        public static IServiceCollection AddWorkflowBackgroundJob(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddSingleton<IWorkflowExecutionManager, WorkflowExecutionManager>();
            services.AddTransient<IWorkflowExecutionContext, WorkflowExecutionContext>();
            services.AddHostedService<QueuedHostedService>();

            return services;
        }
    }
}