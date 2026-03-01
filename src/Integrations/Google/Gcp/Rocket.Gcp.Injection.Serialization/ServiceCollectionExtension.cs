using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Gcp.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGcpJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, GcpExtractWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, GcpExtractExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, GcpConnectorDiscriminator>();

            return services;
        }
    }
}