using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }
    }
}