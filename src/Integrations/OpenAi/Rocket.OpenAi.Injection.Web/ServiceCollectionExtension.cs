using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.OpenAi.Injection.Serialization;

namespace Rocket.OpenAi.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOpenAiWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, OpenAiConnectorProduct>()
                .AddTransient<ISkuWorkflow, OpenAiExtractTextWorkflowProduct>()
                .AddTransient<ISkuWorkflow, OpenAiExtractProjectWorkflowProduct>()
                .AddTransient<ISkuWorkflow, OpenAiRetitleFileWorkflowProduct>();

            services
                .AddOpenAiJsonSerialization();

            return services;
        }
    }
}