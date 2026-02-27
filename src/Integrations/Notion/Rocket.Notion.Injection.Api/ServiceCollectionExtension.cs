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
                .AddTransient<IIntegrationHook, NotionProjectTaskUploadHook>();

            services
                .AddTransient<IWorkflowStepModelMapper, NotionUploadNoteWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, NotionUploadProjectTaskWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, NotionUploadNoteExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, NotionUploadProjectTaskExecutionStepMapper>();

            services
                .AddTransient<IStepModelCloner, NotionUploadNoteStepCloner>()
                .AddTransient<IStepModelCloner, NotionUploadProjectTaskStepCloner>();

            services
                .AddTransient<INotionNoteSearcher, NotionNoteSearcher>()
                .AddTransient<INotionDataSourceSearcher, NotionDataSourceSearcher>();

            services
                .AddTransient<INotionNoteUploader, NotionNoteUploader>()
                .AddTransient<INotionImageUploader, NotionImageUploader>()
                .AddTransient<INotionDataSourceUploader, NotionDataSourceUploader>();

            services
                .AddTransient<IConnectorModelMapper, NotionConnectorMapper>()
                .AddTransient<IBsonMapper, NotionBsonMapper>();

            return services;
        }
    }
}