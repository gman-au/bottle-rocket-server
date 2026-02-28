using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Microsofts.Injection.Serialization;

namespace Rocket.Microsofts.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMicrosoftWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, MicrosoftCustomConnectorProduct>()
                .AddTransient<ISkuConnector, MicrosoftPreMadeConnectorProduct>()
                .AddTransient<ISkuWorkflow, OneDriveUploadWorkflowProduct>()
                .AddTransient<ISkuWorkflow, OneNoteUploadWorkflowProduct>();

            services
                .AddMicrosoftJsonSerialization();

            return services;
        }
    }
}