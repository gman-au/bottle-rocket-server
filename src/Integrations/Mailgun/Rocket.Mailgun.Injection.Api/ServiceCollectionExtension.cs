using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Mailgun.Infrastructure;
using Rocket.Mailgun.Injection.Serialization;

namespace Rocket.Mailgun.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMailgunApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, MailgunSendEmailHook>()
                .AddTransient<IConnectorModelMapper, MailgunConnectorMapper>()
                .AddTransient<IWorkflowStepModelMapper, MailgunSendEmailWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, MailgunSendEmailExecutionStepMapper>()
                .AddTransient<IStepModelCloner, MailgunSendEmailStepCloner>()
                .AddTransient<IMailgunEmailSender, MailgunEmailSender>()
                .AddTransient<IBsonMapper, MailgunBsonMapper>();

            services
                .AddMailgunJsonSerialization();

            return services;
        }
    }
}