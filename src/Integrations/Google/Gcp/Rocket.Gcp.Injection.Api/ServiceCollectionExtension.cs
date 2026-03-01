using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Gcp.Infrastructure;
using Rocket.Gcp.Injection.Serialization;

namespace Rocket.Gcp.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGcpApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, GcpOcrExtractHook>()
                .AddTransient<IWorkflowStepModelMapper, GcpExtractWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, GcpExtractExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, GcpConnectorMapper>()
                .AddTransient<IStepModelCloner, GcpExtractStepCloner>()
                .AddTransient<IVisionOcrService, VisionOcrService>()
                .AddTransient<IBsonMapper, GcpBsonMapper>();

            services
                .AddGcpJsonSerialization();

            return services;
        }
    }
}