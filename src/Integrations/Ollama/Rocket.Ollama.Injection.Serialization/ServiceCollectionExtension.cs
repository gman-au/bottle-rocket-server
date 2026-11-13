using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Ollama.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOllamaJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OllamaExtractTextWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OllamaExtractProjectWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OllamaRetitleFileWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OllamaExtractTextExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OllamaExtractProjectExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OllamaRetitleFileExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, OllamaConnectorDiscriminator>();

            return services;
        }
    }
}