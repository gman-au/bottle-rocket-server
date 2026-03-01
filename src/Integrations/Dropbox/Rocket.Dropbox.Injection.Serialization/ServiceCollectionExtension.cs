using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDropboxJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, DropboxUploadWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, DropboxUploadExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, DropboxConnectorDiscriminator>();

            return services;
        }
    }
}