using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Postmark.Infrastructure;
using Rocket.Postmark.Injection.Serialization;

namespace Rocket.Postmark.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPostmarkApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, PostmarkSendEmailHook>()
                .AddTransient<IConnectorModelMapper, PostmarkConnectorMapper>()
                .AddTransient<IWorkflowStepModelMapper, PostmarkSendEmailWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, PostmarkSendEmailExecutionStepMapper>()
                .AddTransient<IStepModelCloner, PostmarkSendEmailStepCloner>()
                .AddTransient<IPostmarkEmailSender, PostmarkEmailSender>()
                .AddTransient<IBsonMapper, PostmarkBsonMapper>();

            services
                .AddPostmarkJsonSerialization();

            return services;
        }
    }
}