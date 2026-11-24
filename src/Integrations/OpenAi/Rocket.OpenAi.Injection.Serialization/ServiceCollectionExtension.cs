using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.OpenAi.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOpenAiJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OpenAiExtractTextWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OpenAiExtractProjectWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OpenAiRetitleFileWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OpenAiExtractWeeklyPlanWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OpenAiExtractTextExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OpenAiExtractProjectExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OpenAiRetitleFileExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OpenAiExtractWeeklyPlanExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, OpenAiConnectorDiscriminator>();

            return services;
        }
    }
}