using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Markdig.Injection.Serialization;

namespace Rocket.Markdig.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMarkdigWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuWorkflow, ConvertToEpubWorkflowProduct>();

            services
                .AddMarkdigEpubJsonSerialization();
            
            return services;
        }
    }
}