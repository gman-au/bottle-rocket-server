using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.Interfaces;
using Rocket.Postmark.Contracts;
using Rocket.Postmark.Domain;
using Rocket.Postmark.Infrastructure.Extensions;

namespace Rocket.Postmark.Infrastructure
{
    public class PostmarkConnectorMapper(
        IObfuscator obfuscator,
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<PostmarkConnector, PostmarkConnectorSpecifics>(serviceProvider)
    {
        public override PostmarkConnector For(PostmarkConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ServerToken = value.ServerToken;
            result.SenderAddress = value.SenderAddress;

            return result;
        }

        public override PostmarkConnectorSpecifics From(PostmarkConnector value)
        {
            var result =
                base
                    .From(value);

            result.ServerToken = obfuscator.Obfuscate(value.ServerToken);
            result.SenderAddress = value.SenderAddress;

            return result;
        }

        public override async Task PreUpdateAsync(PostmarkConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.ServerToken))
                throw new RocketException(
                    "No server token was provided.",
                    ApiStatusCodeEnum.ValidationError
                );
            
            if (!value.SenderAddress.IsValidEmail())
                throw new RocketException(
                    "Sender address is not a valid email address.",
                    ApiStatusCodeEnum.ValidationError
                );
        }
    }
}