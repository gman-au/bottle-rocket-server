using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Notion.Infrastructure;

namespace Rocket.Notion.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotionIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, NotionHook>()
                .AddTransient<IWorkflowStepModelMapper, NotionUploadWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, NotionUploadExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, NotionConnectorMapper>()
                .AddTransient<IStepModelCloner, NotionUploadStepCloner>()
                .AddTransient<INotionClient, NotionClient>()
                .AddTransient<IBsonMapper, NotionBsonMapper>();

            return services;
        }
    }
}