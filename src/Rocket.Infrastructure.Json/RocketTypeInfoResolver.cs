using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Rocket.Api.Contracts.Connectors;
using Rocket.Api.Contracts.Executions;
using Rocket.Api.Contracts.Workflows;

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

                        if (typeInfo.Type == typeof(ExecutionStepSummary))
                        {
                            typeInfo.PolymorphismOptions =
                                new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type"
                                };
                            
                            foreach (var kvp in ExecutionStepTypeDiscriminatorMap.TypeDiscriminatorMap)
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

                        if (typeInfo.Type == typeof(ConnectorSummary))
                        {
                            typeInfo.PolymorphismOptions =
                                new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type"
                                };
                            
                            foreach (var kvp in ConnectorTypeDiscriminatorMap.TypeDiscriminatorMap)
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
                new UpdateWorkflowStepRequestConverter(),
                new CreateConnectorRequestConverter()
            }
        };
    }
}