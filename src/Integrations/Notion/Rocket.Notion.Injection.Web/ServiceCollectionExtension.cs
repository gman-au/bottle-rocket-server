using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Notion.Injection.Serialization;

namespace Rocket.Notion.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotionWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, NotionConnectorProduct>()
                .AddTransient<ISkuWorkflow, NotionUploadNoteWorkflowProduct>()
                .AddTransient<ISkuWorkflow, NotionUploadProjectTaskWorkflowProduct>();

            services
                .AddNotionJsonSerialization();

            return services;
        }
    }
}