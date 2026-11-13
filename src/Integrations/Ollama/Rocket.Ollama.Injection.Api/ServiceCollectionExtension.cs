using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Ollama.Infrastructure;
using Rocket.Ollama.Infrastructure.Project;
using Rocket.Ollama.Infrastructure.Retitle;
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
                .AddTransient<IIntegrationHook, OllamaExtractProjectHook>()
                .AddTransient<IIntegrationHook, OllamaRetitleFileHook>();
            
            services
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractProjectWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OllamaRetitleFileWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, OllamaExtractTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaExtractProjectExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaRetitleFileExecutionStepMapper>();
            
            services
                .AddTransient<IStepModelCloner, OllamaExtractTextStepCloner>()
                .AddTransient<IStepModelCloner, OllamaExtractProjectStepCloner>()
                .AddTransient<IStepModelCloner, OllamaRetitleFileStepCloner>();
            
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