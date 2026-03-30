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
                .AddTransient<IWorkflowStepModelMapper, LocalUploadWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, LocalUploadExecutionStepMapper>()
                .AddTransient<IStepModelCloner, LocalUploadStepCloner>()
                .AddTransient<IBsonMapper, LocalBsonMapper>();

            services
                .AddLocalJsonSerialization();

            return services;
        }
    }
}