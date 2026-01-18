using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Workflows;

namespace Rocket.Infrastructure.Json
{
    public class UpdateWorkflowStepRequestConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            return typeToConvert.GetGenericTypeDefinition() == typeof(UpdateWorkflowStepRequest<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var stepType = typeToConvert.GetGenericArguments()[0];

            var converterType = typeof(UpdateWorkflowStepRequestConverterInner<>)
                .MakeGenericType(stepType);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        private class UpdateWorkflowStepRequestConverterInner<T> : JsonConverter<UpdateWorkflowStepRequest<T>>
            where T : WorkflowStepSummary
        {
            public override UpdateWorkflowStepRequest<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Deserialization logic if needed
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, UpdateWorkflowStepRequest<T> value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteString(
                    "workflow_id",
                    value.WorkflowId
                );
                writer.WriteString(
                    "workflow_step_id",
                    value.WorkflowStepId
                );
                writer.WriteString(
                    "connection_id",
                    value.ConnectionId
                );

                writer.WritePropertyName("step");
                writer.WriteStartObject();

                // Use the shared mapping
                var stepTypeName =
                    value
                        .Step
                        .GetType()
                        .GetDiscriminator();

                writer.WriteString(
                    "$type",
                    stepTypeName
                );

                var stepJson = JsonSerializer.SerializeToElement(
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