using System;
using System.Threading.Tasks;
using Rocket.Infrastructure.Mapping;
using Rocket.Interfaces;
using Rocket.Postmark.Contracts;
using Rocket.Postmark.Domain;

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

            //result.IntegrationSecret = value.IntegrationSecret;

            return result;
        }

        public override PostmarkConnectorSpecifics From(PostmarkConnector value)
        {
            var result =
                base
                    .From(value);

            /*result.IntegrationSecret = 
                obfuscator
                    .Obfuscate(value.IntegrationSecret);*/

            return result;
        }

        public override async Task PreUpdateAsync(PostmarkConnectorSpecifics value)
        {
            /*if (string.IsNullOrEmpty(value.IntegrationSecret))
                throw new RocketException(
                    "No integration secret was provided.",
                    ApiStatusCodeEnum.ValidationError
                );*/
        }
    }
}