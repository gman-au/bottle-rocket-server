using System.Text.Json;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class SchemaResponseBuilder(ISchemaDictionary schemaDictionary) : ISchemaResponseBuilder
    {
        public object Build(
            string messageContent,
            RocketbookPageTemplateTypeEnum pageTemplateType
        )
        {
            if (string.IsNullOrEmpty(messageContent))
                throw new RocketException(
                    "No OCR data was extracted from the image.",
                    ApiStatusCodeEnum.ThirdPartyServiceError
                );
            
            var schemaType =
                schemaDictionary
                    .From(pageTemplateType);

            if (schemaType == typeof(string))
                return messageContent;

            var data =
                JsonSerializer
                    .Deserialize(
                        messageContent,
                        schemaType
                    );

            return data;
        }
    }
}