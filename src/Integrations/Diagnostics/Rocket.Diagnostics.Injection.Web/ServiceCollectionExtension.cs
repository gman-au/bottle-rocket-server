using Microsoft.Extensions.DependencyInjection;
using Rocket.Diagnostics.Injection.Serialization;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDiagnosticWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuWorkflow, HelloWorldTextWorkflowProduct>()
                .AddTransient<ISkuWorkflow, HelloWorldProjectWorkflowProduct>();

            services
                .AddDiagnosticsJsonSerialization();

            return services;
        }
    }
}