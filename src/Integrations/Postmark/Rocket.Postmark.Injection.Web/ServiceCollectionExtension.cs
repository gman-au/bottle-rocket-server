using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Postmark.Injection.Serialization;

namespace Rocket.Postmark.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPostmarkWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, PostmarkConnectorProduct>()
                .AddTransient<ISkuWorkflow, PostmarkSendEmailWorkflowProduct>();

            services
                .AddPostmarkJsonSerialization();

            return services;
        }
    }
}