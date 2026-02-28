using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Rocket.Api.Contracts.Connectors;
using Rocket.Api.Contracts.Executions;
using Rocket.Api.Contracts.Workflows;
using Rocket.Domain.Connectors;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Json
{
    public class RocketJsonResolverInstanceProvider
        : IJsonResolverInstanceProvider
    {
        private readonly IJsonTypeInfoResolver _defaultJsonTypeInfoResolver;
        private readonly JsonSerializerOptions _defaultSerializerOptions;

        public RocketJsonResolverInstanceProvider(
            IEnumerable<IJsonTypeDiscriminator<BaseWorkflowStep>> workflowTypeDiscriminators,
            IEnumerable<IJsonTypeDiscriminator<BaseExecutionStep>> executionTypeDiscriminators,
            IEnumerable<IJsonTypeDiscriminator<BaseConnector>> connectorTypeDiscriminators,
            CreateWorkflowStepRequestConverter createWorkflowStepRequestConverter,
            UpdateWorkflowStepRequestConverter updateWorkflowStepRequestConverter,
            CreateConnectorRequestConverter createConnectorRequestConverter
        )
        {
            _defaultJsonTypeInfoResolver =
                BuildInstance(
                    workflowTypeDiscriminators,
                    executionTypeDiscriminators,
                    connectorTypeDiscriminators
                );
            
            _defaultSerializerOptions =
                new JsonSerializerOptions
                {
                    TypeInfoResolver = _defaultJsonTypeInfoResolver,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters =
                    {
                        createWorkflowStepRequestConverter,
                        updateWorkflowStepRequestConverter,
                        createConnectorRequestConverter
                    }
                };
        }

        public IJsonTypeInfoResolver GetInstance() => _defaultJsonTypeInfoResolver;
        public JsonSerializerOptions GetSerializationOptions() => _defaultSerializerOptions;

        private static DefaultJsonTypeInfoResolver BuildInstance(
            IEnumerable<IJsonTypeDiscriminator<BaseWorkflowStep>> workflowTypeDiscriminators,
            IEnumerable<IJsonTypeDiscriminator<BaseExecutionStep>> executionTypeDiscriminators,
            IEnumerable<IJsonTypeDiscriminator<BaseConnector>> connectorTypeDiscriminators
        )
        {
            return
                new DefaultJsonTypeInfoResolver
                {
                    Modifiers =
                    {
                        typeInfo =>
                        {
                            if (typeInfo.Type == typeof(WorkflowStepSummary))
                            {
                                typeInfo.PolymorphismOptions =
                                    new JsonPolymorphismOptions
                                    {
                                        TypeDiscriminatorPropertyName = "$type"
                                    };

                                // Add derived types to the collection
                                foreach (var kvp in workflowTypeDiscriminators)
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

                                foreach (var kvp in executionTypeDiscriminators)
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

                                foreach (var kvp in connectorTypeDiscriminators)
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
        }
    }
}