using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.QuestPdf.Infrastructure;

namespace Rocket.QuestPdf.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQuestPdfApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, ConvertToPdfHook>()
                .AddTransient<IWorkflowStepModelMapper, ConvertToPdfWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, ConvertToPdfExecutionStepMapper>()
                .AddTransient<IStepModelCloner, ConvertToPdfStepCloner>()
                .AddTransient<IPdfGenerator, PdfGenerator>()
                .AddTransient<IBsonMapper, QuestPdfBsonMapper>();

            return services;
        }
    }
}