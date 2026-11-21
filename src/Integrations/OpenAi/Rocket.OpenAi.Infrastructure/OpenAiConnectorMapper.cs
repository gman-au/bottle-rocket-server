using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.OpenAi.Contracts;
using Rocket.OpenAi.Domain;

namespace Rocket.OpenAi.Infrastructure
{
    public class OpenAiConnectorMapper(
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<OpenAiConnector, OpenAiConnectorSpecifics>(serviceProvider)
    {
        public override OpenAiConnector For(OpenAiConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ApiKey = value.ApiKey;

            return result;
        }

        public override OpenAiConnectorSpecifics From(OpenAiConnector value)
        {
            var result =
                base
                    .From(value);

            result.ApiKey = value.ApiKey;

            return result;
        }

        public override async Task PreUpdateAsync(OpenAiConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.ApiKey))
                throw new RocketException(
                    "No API key was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}