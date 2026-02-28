using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Ollama.Infrastructure;
using Rocket.Ollama.Infrastructure.Project;
using Rocket.Ollama.Infrastructure.Text;
using Rocket.Ollama.Injection.Serialization;

namespace Rocket.Ollama.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOllamaApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, OllamaExtractTextHook>()
                .AddTransient<IIntegrationHook, OllamaExtractProjectHook>();
            
            services
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractProjectWorkflowStepMapper>();
            
            services
                .AddTransient<IExecutionStepModelMapper, OllamaExtractTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaExtractProjectExecutionStepMapper>();
            
            services
                .AddTransient<IStepModelCloner, OllamaExtractTextStepCloner>()
                .AddTransient<IStepModelCloner, OllamaExtractProjectStepCloner>();
            
            services
                .AddTransient<IConnectorModelMapper, OllamaConnectorMapper>()
                .AddTransient<IOllamaClient, OllamaClient>()
                .AddTransient<IBsonMapper, OllamaBsonMapper>();

            services
                .AddOllamaJsonSerialization();

            return services;
        }
    }
}