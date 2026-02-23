using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.Interfaces;
using Rocket.Replicate.Contracts;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Infrastructure
{
    public class ReplicateConnectorMapper(
        IObfuscator obfuscator,
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<ReplicateConnector, ReplicateConnectorSpecifics>(serviceProvider)
    {
        public override ReplicateConnector For(ReplicateConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ApiToken = value.ApiToken;

            return result;
        }

        public override ReplicateConnectorSpecifics From(ReplicateConnector value)
        {
            var result =
                base
                    .From(value);

            result.ApiToken = 
                obfuscator
                    .Obfuscate(value.ApiToken);

            return result;
        }

        public override async Task PreUpdateAsync(ReplicateConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.ApiToken))
                throw new RocketException(
                    "No API token was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}