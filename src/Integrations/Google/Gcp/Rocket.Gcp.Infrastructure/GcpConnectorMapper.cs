using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Gcp.Contracts;
using Rocket.Gcp.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpConnectorMapper(
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<GcpConnector, GcpConnectorSpecifics>(serviceProvider)
    {
        public override GcpConnector For(GcpConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            // TODO: convert the base64 string into GcpCredential domain entity

            return result;
        }

        public override GcpConnectorSpecifics From(GcpConnector value)
        {
            var result =
                base
                    .From(value);
            
            // don't return the credentials

            return result;
        }

        public override async Task PreUpdateAsync(GcpConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.CredentialsFileBase64))
                throw new RocketException(
                    "No credentials file was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}