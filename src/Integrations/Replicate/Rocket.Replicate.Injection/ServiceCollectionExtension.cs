using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Replicate.Infrastructure;
using Rocket.Replicate.Infrastructure.Models;
using Rocket.Replicate.Infrastructure.Models.DataLabTo;
using Rocket.Replicate.Infrastructure.Models.DeepSeekOcr;
using DataLabToExtractTextHook = Rocket.Replicate.Infrastructure.Models.DataLabTo.DataLabToExtractTextHook;

namespace Rocket.Replicate.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReplicateIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, DataLabToExtractTextHook>()
                .AddTransient<IIntegrationHook, DeepSeekOcrExtractTextHook>();
            
            services
                .AddTransient<IWorkflowStepModelMapper, DataLabToExtractTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, DeepSeekOcrExtractTextWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, DataLabToExtractTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, DeepSeekOcrExtractTextExecutionStepMapper>();

            services
                .AddTransient<IStepModelCloner, DataLabToExtractTextStepCloner>()
                .AddTransient<IStepModelCloner, DeepSeekOcrExtractTextStepCloner>();
            
            services
                .AddTransient<IBsonMapper, DataLabToBsonMapper>()
                .AddTransient<IBsonMapper, DeepSeekBsonMapper>();
            
            services
                .AddTransient<IConnectorModelMapper, ReplicateConnectorMapper>()
                .AddTransient<IBsonMapper, ReplicateBsonMapper>()
                .AddTransient<IReplicateClient, ReplicateClient>();

            return services;
        }
    }
}