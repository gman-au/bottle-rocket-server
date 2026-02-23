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
                .AddTransient<IIntegrationHook, OllamaHook>()
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaExtractExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, OllamaConnectorMapper>()
                .AddTransient<IStepModelCloner, OllamaExtractStepCloner>()
                .AddTransient<IBsonMapper, OllamaBsonMapper>();

            return services;
        }
    }
}