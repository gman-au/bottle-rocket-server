using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;

namespace Rocket.Notion.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotionWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, NotionConnectorProduct>()
                .AddTransient<ISkuWorkflow, NotionUploadWorkflowProduct>();

            return services;
        }
    }
}