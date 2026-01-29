using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class MicrosoftConnectorMapper(
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<MicrosoftConnector, MicrosoftConnectorSpecifics>(serviceProvider)
    {
        public override MicrosoftConnector For(MicrosoftConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ClientId = value.ClientId;
            result.TenantId = value.TenantId;

            return result;
        }

        public override MicrosoftConnectorSpecifics From(MicrosoftConnector value)
        {
            var result =
                base
                    .From(value);

            // don't return sensitive data
            
            return result;
        }

        public override async Task PreUpdateAsync(MicrosoftConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.ClientId))
                throw new RocketException(
                    "No client ID was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
            
            if (string.IsNullOrEmpty(value.TenantId))
                throw new RocketException(
                    "No tenant ID was provided.",
                    ApiStatusCodeEnum.ValidationError
                );            
        }
    }
}