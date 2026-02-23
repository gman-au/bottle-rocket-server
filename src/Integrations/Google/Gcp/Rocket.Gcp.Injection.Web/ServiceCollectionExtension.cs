using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;

namespace Rocket.Gcp.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGcpWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, GcpConnectorProduct>()
                .AddTransient<ISkuWorkflow, GcpExtractWorkflowProduct>();

            return services;
        }
    }
}