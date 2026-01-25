using Microsoft.Extensions.DependencyInjection;
using Rocket.Dropbox.Infrastructure;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDropboxIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, DropboxHook>()
                .AddTransient<IDropboxClientManager, DropboxClientManager>()
                .AddTransient<IWorkflowStepModelMapper, DropboxUploadWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, DropboxUploadExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, DropboxConnectorMapper>()
                .AddTransient<IStepModelCloner, DropboxUploadStepCloner>()
                .AddTransient<IBsonMapper, DropboxBsonMapper>();

            return services;
        }
    }
}