using Microsoft.Extensions.DependencyInjection;
using Rocket.Diagnostics.Domain;
using Rocket.Diagnostics.Infrastructure;
using Rocket.Diagnostics.Injection.Serialization;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDiagnosticApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, HelloWorldTextHook>()
                .AddTransient<IIntegrationHook, HelloWorldProjectHook>()
                .AddTransient<IIntegrationHook, HelloWorldMarkdownHook>();

            services
                .AddTransient<IWorkflowStepModelMapper, HelloWorldTextWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, HelloWorldProjectWorkflowStepMapper>()
                .AddTransient<IWorkflowStepModelMapper, HelloWorldMarkdownWorkflowStepMapper>();

            services
                .AddTransient<IExecutionStepModelMapper, HelloWorldTextExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, HelloWorldProjectExecutionStepMapper>()
                .AddTransient<IExecutionStepModelMapper, HelloWorldMarkdownExecutionStepMapper>();

            services
                .AddTransient<IStepModelCloner, HelloWorldTextStepCloner>()
                .AddTransient<IStepModelCloner, HelloWorldProjectStepCloner>()
                .AddTransient<IStepModelCloner, HelloWorldMarkdownStepCloner>();

            services
                .AddTransient<IBsonMapper, DiagnosticsBsonMapper>();

            services
                .AddDiagnosticsJsonSerialization();

            return services;
        }
    }
}