using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using Rocket.Domain.Transcriptions;
using Rocket.Domain.Transcriptions.Projects;
using Rocket.Domain.Transcriptions.Texts;
using Rocket.Domain.Transcriptions.WeeklyPlans;
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
                .AddSingleton<IJsonTypeDiscriminator<BaseTranscription>, ProjectTranscriptionDiscriminator>()
                .AddSingleton<IJsonTypeDiscriminator<BaseTranscription>, WeeklyPlanTranscriptionDiscriminator>();
            
            services
                .AddTransient<ITranscriptionModelMapper, TextTranscriptionMapper>()
                .AddTransient<ITranscriptionModelMapper, ProjectTranscriptionMapper>()
                .AddTransient<ITranscriptionModelMapper, WeeklyPlanTranscriptionMapper>();
                
            return services;
        }
        
        public static IServiceCollection AddTranscriptionBsonSerialization(this IServiceCollection services)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TextTranscription)))
            {
                BsonClassMap.RegisterClassMap<TextTranscription>(cm =>
                {
                    cm.AutoMap();
                });
            }           
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(ProjectTranscription)))
            {
                BsonClassMap.RegisterClassMap<ProjectTranscription>(cm =>
                {
                    cm.AutoMap();
                });
            }     
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(WeeklyPlanTranscription)))
            {
                BsonClassMap.RegisterClassMap<WeeklyPlanTranscription>(cm =>
                {
                    cm.AutoMap();
                });
            }
            
            return services;
        }
    }
}