using System;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Mapping;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Mailgun.Contracts;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunConnectorMapper(
        IObfuscator obfuscator,
        IServiceProvider serviceProvider
    )
        : ConnectorModelMapperBase<MailgunConnector, MailgunConnectorSpecifics>(serviceProvider)
    {
        public override MailgunConnector For(MailgunConnectorSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ApiKey = value.ApiKey;
            result.SenderAddress = value.SenderAddress;

            return result;
        }

        public override MailgunConnectorSpecifics From(MailgunConnector value)
        {
            var result =
                base
                    .From(value);

            result.ApiKey = obfuscator.Obfuscate(value.ApiKey);
            result.SenderAddress = value.SenderAddress;

            return result;
        }

        public override async Task PreUpdateAsync(MailgunConnectorSpecifics value)
        {
            if (string.IsNullOrEmpty(value.ApiKey))
                throw new RocketException(
                    "No API key was provided.",
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