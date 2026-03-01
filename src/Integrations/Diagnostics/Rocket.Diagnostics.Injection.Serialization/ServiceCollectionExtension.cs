using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDiagnosticsJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, HelloWorldTextWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, HelloWorldTextExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, HelloWorldProjectWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, HelloWorldProjectExecutionDiscriminator>();

            return services;
        }
    }
}