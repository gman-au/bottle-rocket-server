using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Gcp.Infrastructure;

namespace Rocket.Gcp.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGcpIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, GcpOcrExtractHook>()
                .AddTransient<IWorkflowStepModelMapper, GcpExtractWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, GcpExtractExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, GcpConnectorMapper>()
                .AddTransient<IStepModelCloner, GcpExtractStepCloner>()
                .AddTransient<IVisionOcrService, VisionOcrService>()
                .AddTransient<IBsonMapper, GcpBsonMapper>();

            return services;
        }
    }
}