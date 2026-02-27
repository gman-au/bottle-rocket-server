using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Notion.Infrastructure;

namespace Rocket.Notion.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotionApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, NotionNoteUploadHook>()
                .AddTransient<IWorkflowStepModelMapper, NotionUploadNoteWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, NotionUploadNoteExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, NotionConnectorMapper>()
                .AddTransient<IStepModelCloner, NotionUploadNoteStepCloner>()
                .AddTransient<INotionNoteSearcher, NotionNoteSearcher>()
                .AddTransient<INotionNoteUploader, NotionNoteUploader>()
                .AddTransient<INotionImageUploader, NotionImageUploader>()
                .AddTransient<IBsonMapper, NotionBsonMapper>();

            return services;
        }
    }
}