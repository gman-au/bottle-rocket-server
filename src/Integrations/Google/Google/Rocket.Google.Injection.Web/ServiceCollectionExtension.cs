using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;

namespace Rocket.Google.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddGoogleWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, GoogleConnectorProduct>()
                .AddTransient<ISkuWorkflow, GoogleDriveUploadWorkflowProduct>();

            return services;
        }
    }
}