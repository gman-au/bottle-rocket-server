using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Notion.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotionJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, NotionUploadProjectTaskWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, NotionUploadNoteWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, NotionUploadProjectTaskExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, NotionUploadNoteExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, NotionConnectorDiscriminator>();

            return services;
        }
    }
}