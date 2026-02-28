using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Replicate.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReplicateJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, DataLabToExtractProjectWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, DataLabToExtractTextWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, DeepSeekOcrExtractTextWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, DataLabToExtractProjectExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, DataLabToExtractTextExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, DeepSeekOcrExtractTextExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, ReplicateConnectorDiscriminator>();

            return services;
        }
    }
}