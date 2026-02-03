using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.QuestPdf.Infrastructure;

namespace Rocket.QuestPdf.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQuestPdfIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, ConvertToPdfHook>()
                .AddTransient<IWorkflowStepModelMapper, ConvertToPdfWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, ConvertToPdfExecutionStepMapper>()
                .AddTransient<IStepModelCloner, ConvertToPdfStepCloner>()
                .AddTransient<IBsonMapper, QuestPdfBsonMapper>();

            return services;
        }
    }
}