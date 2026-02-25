using Microsoft.Extensions.DependencyInjection;
using Rocket.Diagnostics.Infrastructure;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDiagnosticApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, HelloWorldTextHook>()
                .AddTransient<IIntegrationHook, HelloWorldProjectHook>();

            services
                .AddTransient<IWorkflowStepModelMapper, HelloWorldTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, HelloWorldProjectWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, HelloWorldTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, HelloWorldProjectExecutionStepMapper>();

            services
                .AddTransient<IStepModelCloner, HelloWorldTextStepCloner>()
                .AddTransient<IStepModelCloner, HelloWorldProjectStepCloner>();

            services
                .AddTransient<IBsonMapper, DiagnosticsBsonMapper>();

            return services;
        }
    }
}