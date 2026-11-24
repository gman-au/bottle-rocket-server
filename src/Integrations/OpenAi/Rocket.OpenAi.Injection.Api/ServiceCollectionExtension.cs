using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.OpenAi.Infrastructure;
using Rocket.OpenAi.Infrastructure.Project;
using Rocket.OpenAi.Infrastructure.Retitle;
using Rocket.OpenAi.Infrastructure.Text;
using Rocket.OpenAi.Infrastructure.WeeklyPlan;
using Rocket.OpenAi.Injection.Serialization;

namespace Rocket.OpenAi.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOpenAiApiIntegration(
            this IServiceCollection services
        )
        {
            services
                .AddTransient<IIntegrationHook, OpenAiExtractTextHook>()
                .AddTransient<IIntegrationHook, OpenAiExtractProjectHook>()
                .AddTransient<IIntegrationHook, OpenAiRetitleFileHook>()
                .AddTransient<IIntegrationHook, OpenAiExtractWeeklyPlanHook>();

            services
                .AddTransient<IWorkflowStepModelMapper, OpenAiExtractTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OpenAiExtractProjectWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OpenAiRetitleFileWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, OpenAiExtractWeeklyPlanWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, OpenAiExtractTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OpenAiExtractProjectExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OpenAiRetitleFileExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, OpenAiExtractWeeklyPlanExecutionStepMapper>();

            services
                .AddTransient<IStepModelCloner, OpenAiExtractTextStepCloner>()
                .AddTransient<IStepModelCloner, OpenAiExtractProjectStepCloner>()
                .AddTransient<IStepModelCloner, OpenAiRetitleFileStepCloner>()
                .AddTransient<IStepModelCloner, OpenAiExtractWeeklyPlanStepCloner>();

            services
                .AddTransient<IConnectorModelMapper, OpenAiConnectorMapper>()
                .AddTransient<IOpenAiClient, OpenAiClient>()
                .AddTransient<IBsonMapper, OpenAiBsonMapper>();

            services
                .AddOpenAiJsonSerialization();

            return services;
        }
    }
}