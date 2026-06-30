using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Mailgun.Injection.Serialization
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMailgunJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseWorkflowStep>, MailgunSendEmailWorkflowDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseExecutionStep>, MailgunSendEmailExecutionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseConnector>, MailgunConnectorDiscriminator>();

            return services;
        }
    }
}