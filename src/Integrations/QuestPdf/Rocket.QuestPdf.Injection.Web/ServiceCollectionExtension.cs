using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;

namespace Rocket.QuestPdf.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQuestPdfWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuWorkflow, ConvertToPdfWorkflowProduct>();

            return services;
        }
    }
}