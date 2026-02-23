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
                .AddTransient<IWorkflowStepModelMapper, HelloWorldTextWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, HelloWorldTextExecutionStepMapper>()
                .AddTransient<IStepModelCloner, HelloWorldTextStepCloner>()
                .AddTransient<IBsonMapper, DiagnosticsBsonMapper>();

            return services;
        }
    }
}