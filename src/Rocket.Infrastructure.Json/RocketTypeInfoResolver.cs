using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Rocket.Api.Contracts.Workflows;
using Rocket.Dropbox.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class RocketTypeInfoResolver
    {
        public static readonly DefaultJsonTypeInfoResolver Instance =
            new()
            {
                Modifiers =
                {
                    static typeInfo =>
                    {
                        if (typeInfo.Type == typeof(WorkflowStepSummary))
                        {
                            typeInfo.PolymorphismOptions =
                                new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type"
                                };

                            // Add derived types to the collection
                            foreach (var kvp in WorkflowStepTypeDiscriminatorMap.TypeDiscriminatorMap)
                            {
                                typeInfo
                                    .PolymorphismOptions
                                    .DerivedTypes
                                    .Add(
                                        new JsonDerivedType(
                                            kvp.Key,
                                            kvp.Value
                                        )
                                    );
                            }
                        }
                    }
                }
            };

        public static readonly JsonSerializerOptions DefaultJsonSerializationOptions = new()
        {
            TypeInfoResolver = Instance,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new CreateWorkflowStepRequestConverter(),
                new UpdateWorkflowStepRequestConverter()
            }
        };

        private static JsonDerivedType Build<T>(string discriminator) => new(
            typeof(T),
            discriminator
        );
    }
}