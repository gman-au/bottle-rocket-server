using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Local.Infrastructure;
using Rocket.Local.Injection.Serialization;

namespace Rocket.Local.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLocalApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, LocalUploadHook>()
                .AddTransient<IIntegrationHook, LocalRetitleFileHook>()
                .AddTransient<IWorkflowStepModelMapper, LocalUploadWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, LocalRetitleFileWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, LocalUploadExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, LocalRetitleFileExecutionStepMapper>()
                .AddTransient<IStepModelCloner, LocalUploadStepCloner>()
                .AddTransient<IStepModelCloner, LocalRetitleFileStepCloner>()
                .AddTransient<IFileWriter, FileWriter>()
                .AddTransient<IBsonMapper, LocalBsonMapper>();

            services
                .AddLocalJsonSerialization();

            return services;
        }
    }
}