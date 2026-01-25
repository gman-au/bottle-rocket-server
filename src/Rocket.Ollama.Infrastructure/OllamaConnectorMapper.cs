using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.Ollama.Contracts;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Infrastructure
{
    public class OllamaConnectorMapper(
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<OllamaConnector, OllamaConnectorSpecifics>(serviceProvider)
    {
        public override OllamaConnector For(OllamaConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.Endpoint = value.Endpoint;

            return result;
        }

        public override OllamaConnectorSpecifics From(OllamaConnector value)
        {
            var result =
                base
                    .From(value);

            result.Endpoint = value.Endpoint;

            return result;
        }

        public override async Task PreUpdateAsync(OllamaConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.Endpoint))
                throw new RocketException(
                    "No endpoint was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}