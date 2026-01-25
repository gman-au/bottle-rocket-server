using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.MaxOcr.Infrastructure;

namespace Rocket.MaxOcr.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMaxOcrIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, MaxOcrHook>()
                .AddTransient<IWorkflowStepModelMapper, MaxOcrExtractWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, MaxOcrExtractExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, MaxOcrConnectorMapper>()
                .AddTransient<IStepModelCloner, MaxOcrExtractStepCloner>()
                .AddTransient<IBsonMapper, MacOcrBsonMapper>();

            return services;
        }
    }
}