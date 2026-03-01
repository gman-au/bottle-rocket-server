using Microsoft.Extensions.DependencyInjection;
using Rocket.Google.Infrastructure;
using Rocket.Google.Injection.Serialization;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGoogleApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, GoogleDriveUploadHook>()
                .AddTransient<IWorkflowStepModelMapper, GoogleDriveUploadWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, GoogleDriveUploadExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, GoogleConnectorMapper>()
                .AddTransient<IStepModelCloner, GoogleDriveUploadStepCloner>()
                .AddTransient<IDriveUploadService, DriveUploadService>()
                .AddTransient<IDriveFolderSearcher, DriveFolderSearcher>()
                .AddTransient<IGoogleTokenAcquirer, GoogleTokenAcquirer>()
                .AddTransient<IBsonMapper, GoogleBsonMapper>();

            services
                .AddGoogleJsonSerialization();

            return services;
        }
    }
}