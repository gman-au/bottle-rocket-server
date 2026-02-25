using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;

namespace Rocket.Ollama.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOllamaWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, OllamaConnectorProduct>()
                .AddTransient<ISkuWorkflow, OllamaExtractTextWorkflowProduct>()
                .AddTransient<ISkuWorkflow, OllamaExtractProjectWorkflowProduct>();

            return services;
        }
    }
}