using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Replicate.Infrastructure;
using Rocket.Replicate.Infrastructure.Models.DataLabTo;
using Rocket.Replicate.Infrastructure.Models.DataLabTo.Project;
using Rocket.Replicate.Infrastructure.Models.DataLabTo.Text;
using Rocket.Replicate.Infrastructure.Models.DeepSeekOcr;
using Rocket.Replicate.Infrastructure.Models.Gpt;
using Rocket.Replicate.Injection.Serialization;
using DataLabToExtractTextHook = Rocket.Replicate.Infrastructure.Models.DataLabTo.Text.DataLabToExtractTextHook;

namespace Rocket.Replicate.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddReplicateApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, DataLabToExtractTextHook>()
                .AddTransient<IIntegrationHook, DataLabToExtractProjectHook>()
                .AddTransient<IIntegrationHook, DeepSeekOcrExtractTextHook>()
                .AddTransient<IIntegrationHook, GptRetitleFileHook>();

            services
                .AddTransient<IWorkflowStepModelMapper, DataLabToExtractTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, DataLabToExtractProjectWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, DeepSeekOcrExtractTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, GptRetitleFileWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, DataLabToExtractTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, DataLabToExtractProjectExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, DeepSeekOcrExtractTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, GptRetitleFileExecutionStepMapper>();

            services
                .AddTransient<IStepModelCloner, DataLabToExtractTextStepCloner>()
                .AddTransient<IStepModelCloner, DataLabToExtractProjectStepCloner>()
                .AddTransient<IStepModelCloner, DeepSeekOcrExtractTextStepCloner>()
                .AddTransient<IStepModelCloner, GptRetitleFileStepCloner>();
            
            services
                .AddTransient<IBsonMapper, DataLabToBsonMapper>()
                .AddTransient<IBsonMapper, DeepSeekBsonMapper>()
                .AddTransient<IBsonMapper, GptBsonMapper>();
            
            services
                .AddTransient<IConnectorModelMapper, ReplicateConnectorMapper>()
                .AddTransient<IBsonMapper, ReplicateBsonMapper>()
                .AddTransient<IReplicateClient, ReplicateClient>();

            services
                .AddReplicateJsonSerialization();

            return services;
        }
    }
}