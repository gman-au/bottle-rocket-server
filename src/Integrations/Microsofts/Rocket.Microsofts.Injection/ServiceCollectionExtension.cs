using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Microsofts.Infrastructure;

namespace Rocket.Microsofts.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMicrosoftIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IConnectorModelMapper, MicrosoftConnectorMapper>()
                .AddTransient<IIntegrationHook, OneDriveHook>()
                .AddTransient<IIntegrationHook, OneNoteHook>()
                .AddTransient<IWorkflowStepModelMapper, OneDriveUploadWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OneNoteUploadWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OneDriveUploadExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OneNoteUploadExecutionStepMapper>()
                .AddTransient<IStepModelCloner, OneDriveUploadStepCloner>()
                .AddTransient<IStepModelCloner, OneNoteUploadStepCloner>()
                .AddTransient<IMicrosoftTokenAcquirer, MicrosoftTokenAcquirer>()
                .AddTransient<IGraphClientProvider, GraphClientProvider>()
                .AddTransient<IOneDriveUploader, OneDriveUploader>()
                .AddTransient<IOneNoteSectionSearcher, OneNoteSectionSearcher>()
                .AddTransient<IOneNoteUploader, OneNoteUploader>()
                .AddTransient<IBsonMapper, MicrosoftBsonMapper>();

            return services;
        }
    }
}