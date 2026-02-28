using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.QuestPdf.Injection.Serialization;

namespace Rocket.QuestPdf.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQuestPdfWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuWorkflow, ConvertToPdfWorkflowProduct>();

            services
                .AddQuestPdfJsonSerialization();
            
            return services;
        }
    }
}