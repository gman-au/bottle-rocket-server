using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;

namespace Rocket.Replicate.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReplicateWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuWorkflow, DataLabToExtractTextWorkflowProduct>()
                .AddTransient<ISkuWorkflow, DataLabToExtractProjectWorkflowProduct>()
                .AddTransient<ISkuWorkflow, DeepSeekOcrExtractTextWorkflowProduct>()
                .AddTransient<ISkuConnector, ReplicateConnectorProduct>();

            return services;
        }
    }
}