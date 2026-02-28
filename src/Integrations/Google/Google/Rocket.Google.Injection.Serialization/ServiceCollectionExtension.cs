using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGoogleJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, GoogleDriveUploadWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, GoogleDriveUploadExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, GoogleConnectorDiscriminator>();

            return services;
        }
    }
}