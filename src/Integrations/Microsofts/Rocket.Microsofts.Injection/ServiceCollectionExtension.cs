using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Microsofts.Infrastructure;

namespace Rocket.Microsofts.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMicrosoftIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, OneDriveHook>()
                .AddTransient<IWorkflowStepModelMapper, OneDriveUploadWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OneDriveUploadExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, MicrosoftConnectorMapper>()
                .AddTransient<IStepModelCloner, OneDriveUploadStepCloner>()
                .AddTransient<IMicrosoftTokenAcquirer, MicrosoftTokenAcquirer>()
                .AddTransient<IOneDriveUploader, OneDriveUploader>()
                .AddTransient<IBsonMapper, MicrosoftBsonMapper>();

            return services;
        }
    }
}