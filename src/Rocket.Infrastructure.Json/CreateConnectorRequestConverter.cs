using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;

namespace Rocket.Infrastructure.Json
{
    public class CreateConnectorRequestConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            return typeToConvert.GetGenericTypeDefinition() == typeof(CreateConnectorRequest<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var stepType = typeToConvert.GetGenericArguments()[0];

            var converterType = typeof(CreateConnectorRequestConverterInner<>)
                .MakeGenericType(stepType);

            return (JsonConverter)Activator.CreateInstance(converterType);
        }

        private class CreateConnectorRequestConverterInner<T> : JsonConverter<CreateConnectorRequest<T>>
            where T : ConnectorSummary
        {
            public override CreateConnectorRequest<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Deserialization logic if needed
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, CreateConnectorRequest<T> value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("connector");
                writer.WriteStartObject();

                // Use the shared mapping
                var stepTypeName =
                    value
                        .Connector
                        .GetType()
                        .GetConnectorTypeDiscriminator();

                writer.WriteString(
                    "$type",
                    stepTypeName
                );

                var stepJson = JsonSerializer.SerializeToElement(
                    value.Connector,
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