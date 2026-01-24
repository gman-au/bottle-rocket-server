using System;
using System.Threading.Tasks;
using Rocket.Domain.Core.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.MaxOcr.Contracts;
using Rocket.MaxOcr.Domain;

namespace Rocket.MaxOcr.Infrastructure
{
    public class MaxOcrConnectorMapper(
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<MaxOcrConnector, MaxOcrConnectorSpecifics>(serviceProvider)
    {
        public override MaxOcrConnector For(MaxOcrConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.Endpoint = value.Endpoint;

            return result;
        }

        public override MaxOcrConnectorSpecifics From(MaxOcrConnector value)
        {
            var result =
                base
                    .From(value);

            result.Endpoint = value.Endpoint;

            return result;
        }

        public override async Task PreUpdateAsync(MaxOcrConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.Endpoint))
                throw new RocketException(
                    "No endpoint was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}