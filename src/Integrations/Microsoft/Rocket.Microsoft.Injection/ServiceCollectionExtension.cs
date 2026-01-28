using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Microsoft.Infrastructure;

namespace Rocket.Microsoft.Injection
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
                .AddTransient<IBsonMapper, MicrosoftBsonMapper>();

            return services;
        }
    }
}