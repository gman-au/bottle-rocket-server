using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Replicate.Infrastructure;
using Rocket.Replicate.Infrastructure.Models;

namespace Rocket.Replicate.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReplicateIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, DataLabExtractTextHook>()
                .AddTransient<IWorkflowStepModelMapper, DataLabToExtractTextWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, DataLabToExtractTextExecutionStepMapper>()
                .AddTransient<IConnectorModelMapper, ReplicateConnectorMapper>()
                .AddTransient<IStepModelCloner, DataLabToExtractTextStepCloner>()
                .AddTransient<IBsonMapper, ReplicateBsonMapper>()
                .AddTransient<IBsonMapper, DataLabToBsonMapper>();

            return services;
        }
    }
}