using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Ollama.Infrastructure;
using Rocket.Ollama.Infrastructure.Project;
using Rocket.Ollama.Infrastructure.Retitle;
using Rocket.Ollama.Infrastructure.Text;
using Rocket.Ollama.Infrastructure.WeeklyPlan;
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
                .AddTransient<IIntegrationHook, OllamaRetitleFileHook>()
                .AddTransient<IIntegrationHook, OllamaExtractWeeklyPlanHook>();
            
            services
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractProjectWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OllamaRetitleFileWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OllamaExtractWeeklyPlanWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, OllamaExtractTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaExtractProjectExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaRetitleFileExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OllamaExtractWeeklyPlanExecutionStepMapper>();
            
            services
                .AddTransient<IStepModelCloner, OllamaExtractTextStepCloner>()
                .AddTransient<IStepModelCloner, OllamaExtractProjectStepCloner>()
                .AddTransient<IStepModelCloner, OllamaRetitleFileStepCloner>()
                .AddTransient<IStepModelCloner, OllamaExtractWeeklyPlanStepCloner>();
            
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