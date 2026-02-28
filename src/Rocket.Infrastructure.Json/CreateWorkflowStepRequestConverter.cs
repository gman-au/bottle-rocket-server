using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Json
{
    public class CreateWorkflowStepRequestConverter(IEnumerable<IJsonTypeDiscriminator<BaseWorkflowStep>> typeDiscriminators)
        : JsonConverterFactory
    {
        private readonly Dictionary<Type, string> _typeDiscriminators =
            typeDiscriminators
                .ToDictionary(
                    o => o.Key,
                    o => o.Value
                );

        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            return typeToConvert.GetGenericTypeDefinition() == typeof(CreateWorkflowStepRequest<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var stepType = typeToConvert.GetGenericArguments()[0];

            var converterType = typeof(CreateWorkflowStepRequestConverterInner<>)
                .MakeGenericType(stepType);

            return (JsonConverter)Activator.CreateInstance(
                converterType,
                _typeDiscriminators
            );
        }

        private class CreateWorkflowStepRequestConverterInner<T>(Dictionary<Type, string> typeDiscriminators)
            : JsonConverter<CreateWorkflowStepRequest<T>>
            where T : WorkflowStepSummary
        {
            public override CreateWorkflowStepRequest<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Deserialization logic if needed
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, CreateWorkflowStepRequest<T> value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteString(
                    "workflow_id",
                    value.WorkflowId
                );
                writer.WriteString(
                    "parent_step_id",
                    value.ParentStepId
                );
                writer.WriteString(
                    "connector_id",
                    value.ConnectorId
                );

                writer.WritePropertyName("step");
                writer.WriteStartObject();

                var stepTypeName =
                    typeDiscriminators
                        .GetValueOrDefault(
                            value.Step.GetType(),
                            "base"
                        );

                writer
                    .WriteString(
                        "$type",
                        stepTypeName
                    );

                var stepJson =
                    JsonSerializer
                        .SerializeToElement(
                            value.Step,
                            options
                        );
                foreach (var property in stepJson.EnumerateObject())
                {
                    property.WriteTo(writer);
                }

                writer.WriteEndObject();
                writer.WriteEndObject();
            }
        }
    }
}