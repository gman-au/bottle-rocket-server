using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Microsofts.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMicrosoftJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OneDriveUploadWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, OneNoteUploadWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OneDriveUploadExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, OneNoteUploadExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, MicrosoftConnectorDiscriminator>();

            return services;
        }
    }
}