using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rocket.Api.Contracts.Connectors;
using Rocket.Domain.Connectors;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Json
{
    public class CreateConnectorRequestConverter(IEnumerable<IJsonTypeDiscriminator<BaseConnector>> typeDiscriminators)
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

            return typeToConvert.GetGenericTypeDefinition() == typeof(CreateConnectorRequest<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var stepType = typeToConvert.GetGenericArguments()[0];

            var converterType = typeof(CreateConnectorRequestConverterInner<>)
                .MakeGenericType(stepType);

            return (JsonConverter)Activator.CreateInstance(
                converterType,
                _typeDiscriminators
            );
        }

        private class CreateConnectorRequestConverterInner<T>(Dictionary<Type, string> typeDiscriminators)
            : JsonConverter<CreateConnectorRequest<T>>
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

                var stepTypeName =
                    typeDiscriminators
                        .GetValueOrDefault(
                            value.Connector.GetType(),
                            "base"
                        );

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