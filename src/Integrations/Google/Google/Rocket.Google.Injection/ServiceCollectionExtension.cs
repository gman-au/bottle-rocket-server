using Microsoft.Extensions.DependencyInjection;
using Rocket.Google.Infrastructure;
using Rocket.Interfaces;

namespace Rocket.Google.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGoogleIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, GoogleDriveUploadHook>()
                .AddTransient<IWorkflowStepModelMapper, GoogleDriveUploadWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, GoogleDriveUploadExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, GoogleConnectorMapper>()
                .AddTransient<IStepModelCloner, GoogleDriveUploadStepCloner>()
                .AddTransient<IDriveUploadService, DriveUploadService>()
                .AddTransient<IGoogleTokenAcquirer, GoogleTokenAcquirer>()
                .AddTransient<IBsonMapper, GoogleBsonMapper>();

            return services;
        }
    }
}