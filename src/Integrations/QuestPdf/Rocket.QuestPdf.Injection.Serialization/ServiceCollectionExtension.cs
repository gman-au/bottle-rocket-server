using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.QuestPdf.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQuestPdfJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, ConvertToPdfWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, ConvertToPdfExecutionDiscriminator>();

            return services;
        }
    }
}