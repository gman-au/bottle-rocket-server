using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Mailgun.Injection.Serialization;

namespace Rocket.Mailgun.Injection.Web
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMailgunWebIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<ISkuConnector, MailgunConnectorProduct>()
                .AddTransient<ISkuWorkflow, MailgunSendEmailWorkflowProduct>();

            services
                .AddMailgunJsonSerialization();

            return services;
        }
    }
}