using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Markdig.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMarkdigEpubJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, ConvertToEpubWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, ConvertToEpubExecutionDiscriminator>();

            return services;
        }
    }
}