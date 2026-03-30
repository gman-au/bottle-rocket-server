using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Local.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLocalJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, LocalUploadWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, LocalUploadExecutionDiscriminator>();

            return services;
        }
    }
}