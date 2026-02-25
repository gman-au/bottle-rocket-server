using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Ollama.Infrastructure;

namespace Rocket.Ollama.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOllamaApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, OllamaExtractTextHook>()
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractTextWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaExtractTextExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, OllamaConnectorMapper>()
                .AddTransient<IStepModelCloner, OllamaExtractTextStepCloner>()
                .AddTransient<IOllamaClient, OllamaClient>()
                .AddTransient<IBsonMapper, OllamaBsonMapper>();

            return services;
        }
    }
}