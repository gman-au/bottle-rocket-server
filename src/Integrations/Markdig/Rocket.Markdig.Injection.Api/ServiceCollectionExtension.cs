using Microsoft.Extensions.DependencyInjection;
using Rocket.Interfaces;
using Rocket.Markdig.Infrastructure;
using Rocket.Markdig.Injection.Serialization;

namespace Rocket.Markdig.Injection.Api
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMarkdigApiIntegration(this IServiceCollection services)
        {
            services
                .AddTransient<IIntegrationHook, ConvertToEpubHook>()
                .AddTransient<IWorkflowStepModelMapper, ConvertToEpubWorkflowStepMapper>()
                .AddTransient<IExecutionStepModelMapper, ConvertToEpubExecutionStepMapper>()
                .AddTransient<IStepModelCloner, ConvertToEpubStepCloner>()
                .AddTransient<IEpubGenerator, EpubGenerator>()
                .AddTransient<IEpubRenderer, EpubRenderer>()
                .AddTransient<IAuthorRetriever, AuthorRetriever>()
                .AddTransient<IBsonMapper, MarkdigBsonMapper>();

            services
                .AddMarkdigEpubJsonSerialization();

            return services;
        }
    }
}