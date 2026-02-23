using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;

namespace Rocket.Dropbox.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDropboxWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, DropboxConnectorProduct>()
                .AddTransient<ISkuWorkflow, DropboxUploadWorkflowProduct>();

            return services;
        }
    }
}