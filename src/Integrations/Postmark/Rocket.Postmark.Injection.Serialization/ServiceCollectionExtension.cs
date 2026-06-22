using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Postmark.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPostmarkJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, PostmarkSendEmailWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, PostmarkSendEmailExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, PostmarkConnectorDiscriminator>();

            return services;
        }
    }
}