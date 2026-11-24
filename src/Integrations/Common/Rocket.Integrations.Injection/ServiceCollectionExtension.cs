using Microsoft.Extensions.DependencyInjection;
using Rocket.Domain.Transcriptions;
using Rocket.Infrastructure.Json.Transcriptions;
using Rocket.Infrastructure.Mapping.Transcriptions;
using Rocket.Interfaces;

namespace Rocket.Integrations.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddTranscriptionJsonSerialization(this IServiceCollection services)
        {
            services
                .AddSingleton<IJsonTypeDiscriminator<BaseTranscription>, TextTranscriptionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseTranscription>, ProjectTranscriptionDiscriminator>();
            
            services
                .AddTransient<ITranscriptionModelMapper, TextTranscriptionMapper>()
                .AddTransient<ITranscriptionModelMapper, ProjectTranscriptionMapper>();
                
            return services;
        }
    }
}