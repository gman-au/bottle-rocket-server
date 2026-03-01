using System;
using NJsonSchema;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class SchemaGenerator : ISchemaGenerator
    {
        public string Generate(Type schemaType)
        {
            var schema =
                JsonSchema
                    .FromType(schemaType);

            var jsonString =
                schema
                    .ToJson();

            return jsonString;
        }
    }
}