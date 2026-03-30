using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Local.Injection.Serialization;

namespace Rocket.Local.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLocalWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuWorkflow, LocalUploadWorkflowProduct>();

            services
                .AddLocalJsonSerialization();

            return services;
        }
    }
}